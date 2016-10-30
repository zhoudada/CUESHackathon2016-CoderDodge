using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using ProtoBuf;

public sealed class MicrobitReceiverClient : IDisposable
{
    private IPEndPoint _endPoint;
    private TcpClient _tcpClient;
    private NetworkStream _stream;

    private int _connected;
    private int _socketConnectCancelled;
    private int _socketWorkerCancelled;
    private bool _disposed;
    private LinkedList<MicrobitData> _receivedData = new LinkedList<MicrobitData>();
    private object _receivedDataLock = new object();
    private int _dataQueueLengthMax = 50;

    public bool Connected
    {
        get
        {
            int connected = Interlocked.Add(ref _connected, 0);
            return (connected != 0);
        }
        private set
        {
            Interlocked.Exchange(ref _connected, value ? 1 : 0);
        }
    }

    private bool SocketConnectCancelled
    {
        get
        {
            int cancelled = Interlocked.Add(ref _socketConnectCancelled, 0);
            return (cancelled != 0);
        }
        set { Interlocked.Exchange(ref _socketConnectCancelled, value ? 1 : 0); }
    }

    private bool SocketWorkerCancelled
    {
        get
        {
            int cancelled = Interlocked.Add(ref _socketWorkerCancelled, 0);
            return (cancelled != 0);
        }
        set { Interlocked.Exchange(ref _socketWorkerCancelled, value ? 1 : 0); }
    }

    public MicrobitReceiverClient(int port)
    {
        Debug.Log("Microbit client is created");
        _endPoint = new IPEndPoint(IPAddress.Loopback, port);
        _connected = 0;
        StartConnect();
    }

    ~MicrobitReceiverClient()
    {
        Dispose(false);
    }

    private Thread _socketConnectThread;
    private void StartConnect()
    {
        if (_tcpClient != null)
        {
            _tcpClient.Close();
        }
        _tcpClient = new TcpClient();
        SocketConnectCancelled = false;
        _socketConnectThread = new Thread(SocketConnect);
        _socketConnectThread.Start();
    }

    private void SocketConnect()
    {
        int retryCount = 0;
        while (true)
        {
            if (SocketConnectCancelled)
            {
                // SocketConnectCancelled
                break;
            }
            try
            {
                Debug.Log("Connecting to server");
                _tcpClient.Connect(_endPoint);
            }
            catch (InvalidOperationException)
            {
                // Failed and retry
                Debug.Log(string.Format("Failed in reading from MMF. Retry {0} in 1s", retryCount++));
                Thread.Sleep(1000);
                continue;
            }
            catch (Exception)
            {
                // Failed and retry
                Debug.Log(string.Format("Failed in connecting to server. Retry {0} in 1s", retryCount++));
                Thread.Sleep(1000);
                continue;
            }
            Debug.Log("Connection Established");
            Connected = true;
            _stream = _tcpClient.GetStream();
            StartSocketWorker();
            break;
        }
    }

    private Thread _socketReaderThread;
    private Thread _socketWriterThread;

    private void StartSocketWorker()
    {
        if (_socketReaderThread != null && _socketReaderThread.IsAlive)
        {
            _socketReaderThread.Join();
        }
        SocketWorkerCancelled = false;
        _socketReaderThread = new Thread(SocketReader);
        _socketReaderThread.Start();
    }

    private void SocketReader()
    {
        if (!Connected)
        {
            return;
        }
        using (StreamReader reader = new StreamReader(_stream))
        {
            while (true)
            {
                if (SocketWorkerCancelled)
                {
                    // Socket Workers cancelled
                    Connected = false;
                    break;
                }
                try
                {
                    //string dataString = reader.ReadLine();
                    //Debug.LogWarning(dataString);
                    //MicrobitData data = JsonConvert.DeserializeObject<MicrobitData>(dataString);
                    MicrobitData data = Serializer.DeserializeWithLengthPrefix<MicrobitData>(_stream, PrefixStyle.Base128);
                    if (data == null)
                    {
                        // Socket Exception or Server Down
                        Connected = false;
                        break;
                    }
                    //Debug.LogWarning(string.Format("Received data: {0}", data));
                    AddRxData(data);
                }
                catch (Exception)
                {
                    // SocketWorkerCancelled or Server Stop
                    Connected = false;
                    break;
                }
            }
        }
        // Check if we need to restart the server
        if (SocketWorkerCancelled)
        {
            // We are intentionally closing down
        }
        else
        {
            // Server stopped for whatever reason. Start reconnecting
            Debug.Log("Server stops. Start reconnecting...");
            StartConnect();
        }
    }

    //private void SocketWriter()
    //{
    //    if (!Connected)
    //    {
    //        return;
    //    }
    //    while (true)
    //    {
    //        if (SocketWorkerCancelled)
    //        {
    //            // Socket worker is cancelled by ourselves
    //            try
    //            {
    //                var commands = _controller.GetCommands();
    //                if (commands != null)
    //                {
    //                    foreach (Command c in commands)
    //                    {
    //                        Serializer.SerializeWithLengthPrefix(_stream, c, PrefixStyle.Base128);
    //                        _stream.Flush();
    //                    }
    //                }
    //            }
    //            catch (Exception)
    //            {
    //                // ignore all exceptions
    //            }
    //            break;
    //        }
    //        //if (_controller.IsCommandAvailable())
    //        //{
    //        Command command = _controller.BlockingGetCommand();
    //        if (command == null)
    //        {
    //            if (SocketWorkerCancelled)
    //            {
    //                // We cancel socketworker ourselves and need to send the final command if any.
    //                continue;
    //            }
    //            else
    //            {
    //                // Server breaks down. Exit directly.
    //                break;
    //            }
    //        }
    //        try
    //        {
    //            Serializer.SerializeWithLengthPrefix(_stream, command, PrefixStyle.Base128);
    //            _stream.Flush();
    //        }
    //        catch (Exception)
    //        {
    //            // Socket exception or server stopped
    //            break;
    //        }
    //        //}
    //    }
    //    // The server may breaks down, but reconnecting to server has been done in SocketReader()
    //}

    private void AddRxData(MicrobitData data)
    {
        lock (_receivedDataLock)
        {
            while (_receivedData.Count > _dataQueueLengthMax)
            {
                _receivedData.RemoveLast();
            }
            _receivedData.AddFirst(data);
        }
    }

    public MicrobitData GetRxData()
    {
        lock (_receivedDataLock)
        {
            if (_receivedData.Count == 0)
            {
                return null;
            }
            MicrobitData data = _receivedData.First.Value;
            _receivedData.RemoveFirst();
            return data;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        Connected = false;
        SocketConnectCancelled = true;
        SocketWorkerCancelled = true;
        if (_socketConnectThread != null && _socketConnectThread.IsAlive)
        {
            _socketConnectThread.Join();
        }
        if (_stream != null)
        {
            _stream.Close();
        }
        if (_tcpClient != null)
        {
            _tcpClient.Close();
        }
        if (_socketReaderThread != null && _socketReaderThread.IsAlive)
        {
            _socketReaderThread.Join();
        }
        Debug.Log("Microbit client Disposing Disposed");

        if (disposing)
        {
            GC.SuppressFinalize(this);
        }
        _disposed = true;
    }
}

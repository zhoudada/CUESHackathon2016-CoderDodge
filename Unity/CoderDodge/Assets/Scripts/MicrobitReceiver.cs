using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

public class MicrobitReceiver: IDisposable
{
    public bool SerialPortReaderCancelled
    {
        get
        {
            int cancelled = Interlocked.Add(ref _serialPortReaderCancelled, 0);
            return (cancelled != 0);
        }
        set
        {
            Interlocked.Exchange(ref _serialPortReaderCancelled, value ? 1 : 0);
        }
    }

    public bool Connected
    {
        get
        {
            int connected = Interlocked.Add(ref _connected, 0);
            return connected != 0;
        }
        set
        {
            Interlocked.Exchange(ref _connected, value ? 1 : 0);
        }
    }

    private int _serialPortReaderCancelled;
    private int _connected;
    private Thread _serialPortThread;
    private bool _disposed;
    private int _dataQueueLengthMax = 50;

    public MicrobitReceiver()
    {
        // Scan all the serial port and find the correct one to connect identified by string
        // If found, connect, set flag and create serial port thread
        // If not found, return straightaway.

    }

    ~MicrobitReceiver()
    {
        Dispose(false);
    }

    private void StartConnectSerialPort()
    {
        // Start the thread
    }

    private void CloseSerialPortReader()
    {
        // Close the thread
    }

    private MicrobitData Parse(byte[] bytes)
    {
        // Parse the bytes
        throw new NotImplementedException();
    }

    private void SerialPortReader()
    {
        while (true)
        {
            if (SerialPortReaderCancelled)
            {
                // The thread is cancelled elsewhere.
                break;
            }
            try
            {
                // Blocking get stream of bytes, parse the bytes and AddRxData.
            }
            // Catch more specific exception
            catch (Exception e)
            {
                // Decide what to do. Maybe break from the while loop and end thread.
            }
        }
    }

    /// <summary>
    /// Dispose function for the IDisposable interface. Don't need to touch this function.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        // Destory the serial port reader thread

        _disposed = true;
    }

    

    
}

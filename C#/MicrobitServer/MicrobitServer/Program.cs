using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO.Ports;
using System.Net;
using System.Threading;
using ProtoBuf;

namespace MicrobitServer
{
    class Program
    {
        private static TcpListener _tcpListener;
        private static SerialPort _serialPort;
        private static int _port = 9999;
        private static int _baudrate = 115200;
        private static int _timeout = 20;

        private static void StartServer()
        {
            _tcpListener.Start();
            Console.WriteLine("Waiting for connection.");
            TcpClient client = _tcpListener.AcceptTcpClient();
            NetworkStream clientStream = client.GetStream();
            Console.WriteLine("Client is connected");
            char[] buffer = new char[50];
            int bufferLength = buffer.Length;
            try
            {
                while (true)
                {
                    Thread.Sleep(_timeout);
                    _serialPort.Read(buffer, 0, bufferLength);
                    MicrobitData data = new MicrobitData();
                    for (int i = 0; i < bufferLength; i++)
                    {
                        char c = buffer[i];
                        bool valid = true;
                        switch (c)
                        {
                            case 'A':
                                data.ButtonA = true;
                                break;

                            case 'B':
                                data.ButtonB = true;
                                break;

                            case 'l':
                                data.Left = true;
                                break;

                            case 'r':
                                data.Right = true;
                                break;

                            case 'f':
                                data.Front = true;
                                break;

                            case 'n':
                                break;

                            default:
                                valid = false;
                                break;
                        }
                        if (valid)
                        {
                            break;
                        }
                    }
                    Serializer.SerializeWithLengthPrefix<MicrobitData>(clientStream, data, PrefixStyle.Base128);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
            finally
            {
                client.Close();
                _tcpListener.Stop();
            }
        }

        static void Main(string[] args)
        {
            string[] ports = SerialPort.GetPortNames();
            char lastChar = 'n';
            if (ports.Length > 0)
            {
                using (_serialPort = new SerialPort(ports[0], _baudrate,
                    Parity.None, 8, StopBits.One))
                {
                    _serialPort.Open();
                    _tcpListener = new TcpListener(IPAddress.Loopback, _port);
                    while (true)
                    {
                        StartServer();
                        //Console.WriteLine("Press y to restart connecting");
                        //var key = Console.ReadKey().Key;
                        //if (key != ConsoleKey.Y)
                        //{
                        //    break;
                        //}
                    }
                }
            }
            else
            {
                Console.WriteLine("No serial port is found.");
            }
            //Console.ReadLine();
        }
    }
}

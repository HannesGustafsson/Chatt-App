﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Google.Protobuf;

namespace desktopClient
{
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 256;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class AsynchronousClient
    {
        // The port number for the remote device.  
        private const int port = 11000;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        public static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        // The response from the remote device.  
        private static String response = String.Empty;
        

        public static void StartClient(string msg, bool isListen)
        {
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // The name of the
                // remote device is "host.contoso.com".  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[3];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                Socket client = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();
                // Send test data to the remote device.  
                
                if (isListen)
                {
                    Console.WriteLine("Starting send, timestamp: " + Program.clientTimestamp);
                    Send(client, msg, false);
                    Console.WriteLine("Waiting on sendDone");
                    sendDone.WaitOne();
                    Program.clientTimestamp = DateTime.Now.Ticks;
                    Console.WriteLine("sendDone is done new timestamp: " + Program.clientTimestamp);
                    

                    // Receive the response from the remote device.  
                    Receive(client);
                    Console.WriteLine("Waiting on reciveDone");
                    receiveDone.WaitOne();
                    Console.WriteLine("reciveDone is done");
                    //foreach (var message in AsynchronousClient.messages.MessageList)
                    //{
                    //    Form1.messageLog.Text = Form1.messageLog.Text + message.Alias + new DateTime(long.Parse(message.Timestamp.ToString())) + Environment.NewLine + message.MessageText;
                    //}
                }
                else
                {
                    Send(client, msg, true);
                    Console.WriteLine("Waiting on sendDone");
                    sendDone.WaitOne();
                    Console.WriteLine("sendDone is done");

                }



                // Write the response to the console.  
                //Console.WriteLine("Stored data is: {0}", Program.messages.ToString());
                //receiveDone.WaitOne();
                //Console.WriteLine("StartClient thinks receiveDone is done");
                Console.WriteLine("Closing sockets...");
                // Release the socket.  
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                Console.WriteLine("sockets closed...");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Receive(Socket client)
        {
            Console.WriteLine("Start of Recive");
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
                Console.WriteLine("Recive was end of line");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            Console.WriteLine("Start of ReciveCallback");
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    Console.WriteLine("ReciveCallback thinks theres more data");
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    Console.WriteLine("ReciveCallback thinks all data has arrived");

                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        Console.WriteLine("ReciveCallback has started parsing data");
                        Program.messages = new GetMessagesResponse();
                        Program.messages = GetMessagesResponse.Parser.ParseJson(state.sb.ToString());
                        Console.WriteLine("ReciveCallback is done parsing");

                    }
                    // Signal that all bytes have been received.  
                    Console.WriteLine("ReciveCallback thinks its done");
                    receiveDone.Set();
                    //Console.WriteLine(receiveDone.GetAccessControl().ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Send(Socket client, String data, bool isInput)
        {
            Console.WriteLine("Start of Send");

            // Convert the string data to byte data using ASCII encoding.  

            BackendRequest request = new BackendRequest();

            request.IsInput = isInput;
            request.Timestamp = Program.clientTimestamp;
            request.Input = new InputMessage
            {
                //IpAddress =  Dns.GetHostAddresses(Dns.GetHostName())[3].Address.ToString(),
                IpAddress = Program.GetBigMac(),                
                MessageToInput = new MessageObject()
                {
                    MessageText = data,
                    Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),                    
                }

                

            };
            

            

            byte[] byteData = Encoding.ASCII.GetBytes(JsonFormatter.Default.Format(request) + "<EOF>");



            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
            sendDone.WaitOne();


            //if (!isInput)
            //{
            //    Receive(client);
            //    Console.WriteLine("Waiting on reciveDone");
            //    receiveDone.WaitOne();
            //    Console.WriteLine("reciveDone is done");
            //}

            //client.BeginSend(byteData, 0, byteData.Length, 0,
            //    new AsyncCallback(SendCallback), client);

        }

        private static void SendCallback(IAsyncResult ar)
        {
            Console.WriteLine("Start of SendCallback");

            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                sendDone.Set();
                //Console.WriteLine(sendDone.GetAccessControl().ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


    }
}

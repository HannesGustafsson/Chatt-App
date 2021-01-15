using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
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
    /// <summary>
    /// The Class for sending and reciving datagrams to/from the server.
    /// </summary>
    public class AsynchronousClient
    {
        // The port number for the remote device.  
        private const int port = 11000;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        public static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        public static ManualResetEvent receiveDone =
            new ManualResetEvent(false);


        /// <summary>
        /// Function for starting the client for connecting and starting send/recive operations.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isListen"></param>
        public static void StartClient(string msg, bool isListen)
        {
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
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

                if (isListen)
                {
                    Console.WriteLine("StartClient thinks the time is: " + Program.clientTimestamp.ToString());
                    Send(client, msg, false);
                    sendDone.WaitOne();
                    Program.clientTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    //Program.clientTimestamp = 1610730982119;
                    Console.WriteLine("StartClient set the time to: " + Program.clientTimestamp.ToString());

                    // Receive the response from server 
                    Receive(client);
                    receiveDone.WaitOne();
                }
                else
                {
                    Send(client, msg, true);
                    sendDone.WaitOne();
                }

                // Close socket 
                client.Shutdown(SocketShutdown.Both);
                client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Callback function for connecting to server
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made 
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Function for reciving data from server
        /// </summary>
        /// <param name="client"></param>
        private static void Receive(Socket client)
        {
            Console.WriteLine("Start of Recive");
            try
            {
                // Create the state object
                StateObject state = new StateObject { workSocket = client };
                

                // Begin receiving the data from server  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
                Console.WriteLine("Recive was end of line");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Recive callback function for reciving data, this function parses the data from server.
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
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
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        Program.messages = new GetMessagesResponse();
                        Program.messages = GetMessagesResponse.Parser.ParseJson(state.sb.ToString());

                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Function for sending requests to the server, this class compiles and sends the protobuff request object to server.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        /// <param name="isInput"></param>
        private static void Send(Socket client, String data, bool isInput)
        {
            Console.WriteLine("Send function thinks the time is: " + Program.clientTimestamp.ToString());
            // Convert the string data to byte data using ASCII encoding.  
            BackendRequest request = new BackendRequest();

            request.IsInput = isInput;
            request.Timestamp = Program.clientTimestamp;
            request.Input = new InputMessage
            {
                IpAddress = Program.GetBigMac(),
                MessageToInput = new MessageObject()
                {
                    MessageText = data,
                    Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                }
            };

            byte[] byteData = Encoding.ASCII.GetBytes(JsonFormatter.Default.Format(request) + "<EOF>");

            // Begin sending the data to the remote device.  
            Console.WriteLine("Send function thinks the time is: " + Program.clientTimestamp.ToString() + " when it starts sending");
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
            sendDone.WaitOne();
            Console.WriteLine("Send function thinks the time is: " + Program.clientTimestamp.ToString() + " when it is done sending");
        }

        /// <summary>
        /// Callback function for sending datagrams to server
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {

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

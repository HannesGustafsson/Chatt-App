using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net; //added
using System.Net.Sockets; //added
using Google.Protobuf;

namespace BackendChattApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ListenTCP();
        }

        static void ListenTCP()
        {
            try
            {
                TcpListener tcpListener = new TcpListener(GetLocalIP(), 11000);
                tcpListener.Start();

                while (true)
                {
                    BackendRequest request = new BackendRequest();
                    Console.Write("Waiting for a connection... ");
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    NetworkStream stream = tcpClient.GetStream();

                    

                    try
                    {
                        request = BackendRequest.Parser.ParseFrom(stream);
                        if (request.IsInput)
                        {
                            AddMessage(request.Input);
                            tcpClient.Close();
                        }
                        else
                        {
                            GetMessagesResponse response = GetMessages(request.Timestamp);
                            Console.Write("Waiting for a connection to transfer... ");
                            tcpClient = tcpListener.AcceptTcpClient();

                            response.WriteTo(tcpClient.GetStream());
                            tcpClient.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static private IPAddress GetLocalIP()
        {
            //Retrives the local computers IP from the DNS
            IPAddress[] iPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            return iPAddresses[3];
        }
        static private void AddMessage(InputMessage message)
        {
            // adds message to DB
        }
        static private GetMessagesResponse GetMessages(Int64 timestamp)
        {
            // Retrives messages from DB
            return new GetMessagesResponse();
        }
    }
}

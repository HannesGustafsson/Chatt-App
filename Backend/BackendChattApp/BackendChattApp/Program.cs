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
            Console.WriteLine("Hello World!");

            ListenTCP();
        }

        static void ListenTCP()
        {
            try
            {
                TcpListener tcpListener = new TcpListener(GetLocalIP(), 11000);
                tcpListener.Start();
                Byte[] bytes = new Byte[256];
                string msgR = null;

                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    msgR = null;

                    NetworkStream stream = tcpClient.GetStream();

                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        msgR = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Recived: {0}", msgR);                        

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes("Thanks i guess?");

                        stream.Write(msg, 0, msg.Length);
                    }
                    tcpClient.Close();
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
    }
}

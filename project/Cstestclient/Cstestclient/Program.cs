using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net; //added
using System.Net.Sockets; //added
using Google.Protobuf;

namespace Cstestclient
{
    class Program
    {
        static void Main(string[] args)
        {
            SendMessage();
            //GetMessage(DateTime.Now.Ticks);
            //GetMessage(0);
            while (true)
            {

            }
        }

        static void SendMessage()
        {
            BackendRequest request = new BackendRequest();

            request.IsInput = true;
            request.Input = new InputMessage()
            {
                IpAddress = Dns.GetHostAddresses(Dns.GetHostName()).ToString(),
                MessageToInput = new MessageObject()
                {
                    MessageText = "Test text",
                    Timestamp = DateTime.Now.Ticks
                }
            };

            TcpClient client = new TcpClient("GamePC", 11000);
            NetworkStream stream = client.GetStream();

            request.WriteTo(stream);
            Console.WriteLine("Got message");

            
            client.Close();
        }

        static void GetMessage(Int64 timestamp)
        {
            BackendRequest request = new BackendRequest();
            GetMessagesResponse response = new GetMessagesResponse();
            request.IsInput = false;
            request.Timestamp = timestamp;
            TcpClient client = new TcpClient("GamePC", 11000);
            NetworkStream stream = client.GetStream();

            request.WriteTo(stream);
            client.Close();

            client = new TcpClient("GamePC", 11000);
            stream = client.GetStream();

            response = GetMessagesResponse.Parser.ParseFrom(stream);

            client.Close();
        }


    }
}

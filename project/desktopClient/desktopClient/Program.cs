using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using Google.Protobuf;



namespace desktopClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void SendMessage ()
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

            TcpClient client = new TcpClient(Dns.GetHostName(), 11000);
            NetworkStream stream = client.GetStream();

            request.WriteTo(stream);
            Console.WriteLine("Got message");


            client.Close();
        }
    }
}

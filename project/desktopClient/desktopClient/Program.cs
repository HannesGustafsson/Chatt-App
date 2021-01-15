using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using Google.Protobuf;
using System.Net.NetworkInformation;

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
            Console.WriteLine("Hello world!");
            clientTimestamp = 0;
            messages = new GetMessagesResponse();
            //AsynchronousClient.StartClient();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


            //AsynchronousClient client = new AsynchronousClient();

        }
        public static GetMessagesResponse messages;
        public static long clientTimestamp;
        public static void SendMessage(string msg)
        {
            AsynchronousClient.StartClient(msg, false);
        }
        public static string GetBigMac()
        {
            return NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault(); ;
        }
    }
}

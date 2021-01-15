using System;
using System.Linq;
using System.Windows.Forms;
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        /// <summary>
        /// Storage varible for messages for Form1 to populate list
        /// </summary>
        public static GetMessagesResponse messages;
        /// <summary>
        /// Storage variable for timestamp for determining latest update of messageLog
        /// </summary>
        public static long clientTimestamp;
        /// <summary>
        /// Starts the AsynchronousClient for sending the message
        /// </summary>
        /// <param name="msg"></param>
        public static void SendMessage(string msg)
        {
            AsynchronousClient.StartClient(msg, false);
        }

        /// <summary>
        /// Returns client's mac-address / Bigmac
        /// </summary>
        public static string GetBigMac()
        {
            return NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault(); ;
        }
        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dtDateTime;
        }
    }
}

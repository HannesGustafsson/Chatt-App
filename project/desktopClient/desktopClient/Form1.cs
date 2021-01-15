using System;
using System.Threading;
using System.Windows.Forms;


namespace desktopClient
{
    /// <summary>
    /// Main Form for the application.
    /// </summary>
    public partial class Form1 : Form
    {        
        public Form1()
        {
            InitializeComponent();
            Populate(); //Begins populating the form with the latest messages sent.
        }
        /// <summary>
        /// Populates the messageLog with the data retrived from the server
        /// </summary>
        public void Populate()
        {
            Console.WriteLine("Populate thinks the time is: " + Program.clientTimestamp.ToString());
            AsynchronousClient.StartClient("", true);
            foreach (var message in Program.messages.MessageList)
            {
                messageLog.Text =  message.Alias + message.Timestamp.ToString() + Environment.NewLine + message.MessageText + Environment.NewLine + messageLog.Text;                
            }
            AsynchronousClient.receiveDone = new ManualResetEvent(false);
            Console.WriteLine("Populate after doing it's thing now thinks the time is: " + Program.clientTimestamp.ToString());
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Send button thinks the time is: " + Program.clientTimestamp.ToString());
            Program.SendMessage(this.messageInput.Text);
            //AsynchronousClient.sendDone.WaitOne();
            ////After the message has been sent, refreshes the messageLog.
            //Populate();
        }

        private void messageInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void messageLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            Populate();
        }
    }
}

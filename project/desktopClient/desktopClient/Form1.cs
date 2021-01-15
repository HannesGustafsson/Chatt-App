using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace desktopClient
{
    public partial class Form1 : Form
    {
        string serverIp = "localhost";
        int port = 2222;

        public Form1()
        {
            InitializeComponent();
            
            
            Populate();
            
            
        }
        public void Populate()
        {
            AsynchronousClient.StartClient("", true);
            messageLog.Text = "";
            foreach (var message in Program.messages.MessageList)
            {
                messageLog.Text = messageLog.Text + message.Alias + new DateTime(long.Parse(message.Timestamp.ToString())) + Environment.NewLine + message.MessageText + Environment.NewLine;
            }
            AsynchronousClient.receiveDone = new ManualResetEvent(false);

        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            Program.SendMessage(this.messageInput.Text);
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
    }
}

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            Heartbeat(); //Starts hearbeat for automatik update
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
                messageLog.Text = message.Alias + " " + Program.UnixTimeToDateTime(1610730982120) + Environment.NewLine + message.MessageText + Environment.NewLine + messageLog.Text;
            }

            AsynchronousClient.receiveDone = new ManualResetEvent(false);
            Console.WriteLine("Populate after doing it's thing now thinks the time is: " + Program.clientTimestamp.ToString());
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            // If Sarcastic is checked, sarcastify message
            if (this.radioButtonSarc.Checked)
            {
                Random random = new Random();
                string s = "";
                foreach (char c in this.messageInput.Text)
                {
                    if (random.Next(0, 2) == 0)
                    {
                        s += char.ToUpper(c).ToString();
                    }
                    else
                    {
                        s += c.ToString();
                    }
                }
                this.messageInput.Text = s;
            }
            // If angry is checked, ragify message
            else if (this.radioButtonAngry.Checked)
            {
                string s = "";
                foreach (char c in this.messageInput.Text)
                {
                    s += char.ToUpper(c).ToString();
                }
                this.messageInput.Text = s + "!!1!";
            }
            // If drunk is checked, ragify message
            else if (this.radioButtonDrunk.Checked)
            {
                Random random = new Random();
                string s = "";
                foreach (char c in this.messageInput.Text)
                {
                    if (random.Next(0, 4) == 0)
                    {
                        s += Convert.ToChar(random.Next(97, 123));
                    }
                    else
                    {
                        s += c.ToString();
                    }
                }
                this.messageInput.Text = s;
            }

            Console.WriteLine("Send button thinks the time is: " + Program.clientTimestamp.ToString());
            Program.SendMessage(this.messageInput.Text);
            this.messageInput.Text = "";
            //After the message has been sent, refreshes the messageLog.
            Populate();
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
        /// <summary>
        /// Heartbeat task for passive updates of messages
        /// </summary>
        /// <returns></returns>
        async Task<bool> Heartbeat()
        {
            bool stopit = false;
            while (!stopit)
            {
                await Task.Delay(5000);
                Console.WriteLine("Heartbeat time!");
                this.Populate();
            }
            return stopit;
        }
    }
}

using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ChattApp
{
    public partial class Client : Form
    {
        Button btnSendMessage;

        TextBox tboxUsername;
        TextBox tboxMessage;
        TextBox tboxInbox;
        public Client()
        {
            InitializeComponent();

            this.Shown += CreateMembersDelegate;

            this.Width = 450;
            this.Height = 400;
        }
        private void CreateMembersDelegate(object sender, EventArgs e)
        {
            // btnSendMessage
            btnSendMessage = new Button();
            this.Controls.Add(btnSendMessage);
            btnSendMessage.Text = "Send Message";
            btnSendMessage.Click += SendMessage;
            btnSendMessage.Width = 75;
            btnSendMessage.Height = 25;
            btnSendMessage.Location = new Point(10, 310);
            btnSendMessage.Enabled = true;
            btnSendMessage.Show();
            //btnStartClient.Show();
            // labUsername
            Label labUsername = new Label();
            this.Controls.Add(labUsername);
            labUsername.Text = "Username";
            labUsername.Location = new Point(10, 25);
            labUsername.Show();
            // tboxUsername
            tboxUsername = new TextBox();
            this.Controls.Add(tboxUsername);
            tboxUsername.Width = 75;
            tboxUsername.Location = new Point(labUsername.Width, 25);
            tboxUsername.BringToFront();
            tboxUsername.Show();
            // labMessage
            Label labMessage = new Label();
            this.Controls.Add(labMessage);
            labMessage.Text = "Message";
            labMessage.Location = new Point(10, 70);
            labMessage.Show();
            // tboxMessage
            tboxMessage = new TextBox();
            this.Controls.Add(tboxMessage);
            tboxMessage.Multiline = true;
            tboxMessage.Width = 150;
            tboxMessage.Height = 200;
            tboxMessage.Location = new Point(10, 100);
            tboxMessage.Show();
            // labInbox
            Label labInbox = new Label();
            this.Controls.Add(labInbox);
            labInbox.Text = "Inbox";
            labInbox.Location = new Point(220, 70);
            labInbox.Show();
            // tboxInbox
            tboxInbox = new TextBox();
            this.Controls.Add(tboxInbox);
            tboxInbox.Multiline = true;
            tboxInbox.Width = 150;
            tboxInbox.Height = 200;
            tboxInbox.Location = new Point(220, 100);
            tboxInbox.Show();
        }
        private void SendMessage(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient("127.0.0.1", 12345);
            using (var w = new BinaryWriter(client.GetStream(), Encoding.UTF8, true))
            {
                w.Write(tboxUsername.Text + ": " + tboxMessage.Text);
            }
            tboxMessage.Text = "";
        }
    }
}

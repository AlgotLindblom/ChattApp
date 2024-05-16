using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace ChattApp
{
    public partial class Client : Form
    {
        IPAddress address = IPAddress.Parse("127.0.0.1");
        int port = 12345;
        TcpClient client;

        Button btnSendMessage = new Button();
        TextBox tboxUsername = new TextBox();
        TextBox tboxMessage = new TextBox();

        public Client()
        {
            InitializeComponent();
            this.Shown += CreateMembersDelegate;

            this.Width = 250;
            this.Height = 400;
        }

        private void CreateMembersDelegate(object sender, EventArgs e)
        {
            // btnSendMessage
            this.Controls.Add(btnSendMessage);
            btnSendMessage.Text = "Send Message";
            btnSendMessage.Click += btnSendMessage_Click;
            btnSendMessage.Width = 75;
            btnSendMessage.Height = 25;
            btnSendMessage.Location = new Point(25, 50);
            btnSendMessage.Show();
            // labUsername
            Label labUsername = new Label();
            this.Controls.Add(labUsername);
            labUsername.Text = "Username";
            labUsername.Location = new Point(25, 25);
            labUsername.Show();
            // tboxUsername
            this.Controls.Add(tboxUsername);
            tboxUsername.Width = 75;
            tboxUsername.Location = new Point(labUsername.Width, 25);
            tboxUsername.BringToFront();
            tboxUsername.Show();
            // tboxMessage
            this.Controls.Add(tboxMessage);
            tboxMessage.Multiline = true;
            tboxMessage.Width = 150;
            tboxMessage.Height = 225;
            tboxMessage.Location = new Point(25, 90);
            tboxMessage.Show();
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            client.NoDelay = true;
            client.Connect(address, port);

            if (client.Connected)
            {
                string outStr = tboxUsername.Text + ": " + tboxMessage.Text;
                byte[] outData = Encoding.Unicode.GetBytes(outStr);
                client.GetStream().Write(outData, 0, outData.Length);
                client.Close();
            }
            else
            {
                MessageBox.Show("CATASTROPHIC FAILURE");
            }
        }
    }
}

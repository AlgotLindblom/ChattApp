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
        int serverPort = 12345;
        int clientPort = 54321;
        TcpClient   client;
        TcpListener listener;

        Button btnSendMessage = new Button();
        Button btnStartClient = new Button();

        TextBox tboxUsername = new TextBox();
        TextBox tboxMessage  = new TextBox();
        TextBox tboxInbox    = new TextBox();

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
            this.Controls.Add(btnSendMessage);
            btnSendMessage.Text = "Send Message";
            btnSendMessage.Click += btnSendMessage_Click;
            btnSendMessage.Width = 75;
            btnSendMessage.Height = 25;
            btnSendMessage.Location = new Point(10, 310);
            btnSendMessage.Enabled = false;
            btnSendMessage.Show();
            // btnStartClient
            this.Controls.Add(btnStartClient);
            btnStartClient.Text = "Start";
            btnStartClient.Width = 75;
            btnStartClient.Height = 25;
            btnStartClient.Location = new Point(200, 25);
            btnStartClient.Click += btnStartClient_Click;
            btnStartClient.Show();
            // labUsername
            Label labUsername = new Label();
            this.Controls.Add(labUsername);
            labUsername.Text = "Username";
            labUsername.Location = new Point(10, 25);
            labUsername.Show();
            // tboxUsername
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
            this.Controls.Add(tboxInbox);
            tboxInbox.Multiline = true;
            tboxInbox.Width = 150;
            tboxInbox.Height = 200;
            tboxInbox.Location = new Point(220, 100);
            tboxInbox.Show();
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            // I think I need to keep the client alive to make my idea work.
            // IDEAS: THreading for recieving and sending.
            client = new TcpClient();
            client.NoDelay = true;
            try
            {
                client.Connect(address, serverPort);
            }
            catch(Exception error){ MessageBox.Show(error.Message); }

            if (client.Connected)
            {
                string outStr = tboxUsername.Text + ": " + tboxMessage.Text;
                byte[] outData = Encoding.Unicode.GetBytes(outStr);
                client.GetStream().Write(outData, 0, outData.Length);
                client.Close(); // None of this.
            }
            else
            {
                MessageBox.Show("Connection failed.");
            }

        }
        private void btnStartClient_Click(object sender, EventArgs e)
        {
            // this should  be fine
            try
            {
                listener = new TcpListener(IPAddress.Any, clientPort);
                listener.Start();
            }
            catch (Exception error) { MessageBox.Show(error.Message, Text); return; }

            btnSendMessage.Enabled = true;
            btnStartClient.Enabled = false;
            Recieve();
        }

        private async void Recieve()
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            while (true)
            {
                HandleClient(client);
            }
        }
        private async void HandleClient(TcpClient c)
        {
            byte[] buff = new byte[1024];
            int n;

            try
            {
                while ((n = await c.GetStream().ReadAsync(buff, 0, buff.Length)) != 0)
                {
                    string message = Encoding.Unicode.GetString(buff, 0, n);
                    LogMessage(message);

                }
            }
            catch (Exception error) { MessageBox.Show(error.Message, Text); }

            c.Close();
        }
        private void LogMessage(string message)
        {
            if (tboxInbox.InvokeRequired)
            {
                tboxInbox.Invoke(new MethodInvoker(delegate { tboxInbox.AppendText(message + Environment.NewLine); }));
            }
            else
            {
                tboxInbox.AppendText(message + Environment.NewLine);
            }
        }
    }
}

using System;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace ChattApp
{
    public partial class Client2 : Form
    {
        Button btnSendMessage;

        TextBox tboxUsername;
        TextBox tboxMessage;
        TextBox tboxInbox;

        TcpClient client = new TcpClient();
        IPEndPoint remoteEndPoint= new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
        //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Client2()
        {
            InitializeComponent();

            this.Shown += CreateMembersDelegate;

            this.Width = 450;
            this.Height = 400;

            Console.WriteLine("Client: Address: {0}, Port: {1}", remoteEndPoint.Address, remoteEndPoint.Port);



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
        private async void SendMessage(object sender, EventArgs e)
        {
            if (!client.Connected)
            {
                try
                {
                    await client.ConnectAsync(remoteEndPoint.Address, remoteEndPoint.Port);
                    Console.WriteLine("Client: Client connected");
                }
                catch(Exception error) { MessageBox.Show(error.Message); return; }
            }
            using (NetworkStream ns = client.GetStream())
            {
                using (var w = new BinaryWriter(ns, Encoding.UTF8, true))
                {
                    w.Write(tboxUsername.Text + ": " + tboxMessage.Text);
                    Console.WriteLine("Client: Message written to stream");
                }
                tboxMessage.Text = "";
                Recieve(ns);
            }
        }
        private void Recieve(NetworkStream ns)
        {
            try
            {
                using (var r = new BinaryReader(ns, Encoding.UTF8, true))
                {
                    Console.WriteLine("Client: Stream read START"); // <- printed
                    tboxInbox.Text = r.ReadString();                // Server doesn't send anything :D
                    Console.WriteLine("Client: Stream read");       // <- not printed
                }
            }
            catch (Exception error) { Console.WriteLine("Client: Stream read failure"); MessageBox.Show(error.Message); return; }
        }
    }
}

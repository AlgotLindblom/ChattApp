using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

// TODO: Alternate server + client that makes a new connection for every message.

namespace ChattApp
{
    public partial class Server : Form
    {
        // Objects need to be in scope. 
        // BUTTON
        Button btnStartServer = new Button();
        // TEXTBOX
        TextBox tbxAddress = new TextBox();
        TextBox tbxInbox = new TextBox();
        public Server()
        {
            InitializeComponent();
            // Control WHEN objects are created
            this.Shown += CreateButtonDelegate;
            this.Shown += CreateTextBoxDelegate;

            this.Width = 450;
            this.Height = 500;
        }
        private void CreateButtonDelegate(object sender, EventArgs e)
        {
            // btnStartServer
            this.Controls.Add(btnStartServer);
            // Set properties of btnStartServer
            btnStartServer.Text = "Start Server";
            btnStartServer.Width = 100;
            btnStartServer.Height = 40;
            btnStartServer.Location = new Point(25, 25);
            // Set Events
            btnStartServer.Click += StartServer;
            btnStartServer.Show();
        }

        private void CreateTextBoxDelegate(object sender, EventArgs e)
        {
            // tbxInbox
            this.Controls.Add(tbxInbox);
            // Set properties of tbxAdress
            tbxInbox.Multiline = true;
            tbxInbox.Location = new Point(25, 90);
            tbxInbox.Width = 350;
            tbxInbox.Height = 350;
            tbxInbox.ScrollBars = ScrollBars.Vertical;
            tbxInbox.Show();
        }
        private async void StartServer(object sender, EventArgs e)
        {
            TcpListener listener;
            try
            {
                listener = new TcpListener(IPAddress.Any, 12345);
                listener.Start();
            }
            catch (Exception error) { MessageBox.Show(error.Message, Text); return; }

            Console.WriteLine("Server: Server started");
            this.btnStartServer.Enabled = false;

            while (true)
            {
                TcpClient clientSocket = await listener.AcceptTcpClientAsync();
                IPEndPoint endPoint = (IPEndPoint)clientSocket.Client.LocalEndPoint;
                Console.WriteLine("Server: Connection to {0} established at {1}", endPoint.Address, endPoint.Port);

                ClientHandler client = new ClientHandler(tbxInbox);
                client.startClient(clientSocket);
            }
        }
    }
    class ClientHandler
    {
        TcpClient client;
        TextBox tbxLog;

        public ClientHandler(TextBox tbx)
        {
            tbxLog = tbx;
        }
        public void startClient(TcpClient inClient)
        {
            this.client = inClient;
            Thread thread = new Thread(stream);
            thread.Start();
        }
        private void stream()
        {
            while (true)
            {
                using (var r = new BinaryReader(client.GetStream(), Encoding.UTF8, true))
                {
                    LogMessage(r.ReadString());
                }
            }
        }
        private void LogMessage(string message)
        {
            // Not fucked.
            Console.WriteLine($"Server: {message}");
            if (tbxLog.InvokeRequired)
            {
                tbxLog.Invoke(new MethodInvoker(delegate { tbxLog.AppendText($"{DateTime.Now}" + message + Environment.NewLine); }));
            }
            else
            {
                tbxLog.AppendText($"{DateTime.Now}" + message + Environment.NewLine);
            }
        }

    }
}

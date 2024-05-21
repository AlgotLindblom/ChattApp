using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace ChattApp
{
    public partial class Server2 : Form
    {
        // Objects need to be in scope. 
        // BUTTON
        Button btnStartServer = new Button();
        // TEXTBOX
        TextBox tbxAddress = new TextBox();
        TextBox tbxInbox = new TextBox();

        TcpListener listener;
        public Server2()
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
            try
            {
                listener = new TcpListener(IPAddress.Any, 12345);
                listener.Start();
                Console.WriteLine("Server: Listener started on port {0}", ((IPEndPoint)listener.LocalEndpoint).Port);
            }
            catch (Exception error) { MessageBox.Show(error.Message, Text); return; }

            Console.WriteLine("Server: Server started");
            this.btnStartServer.Enabled = false;

            while (true)
            {
                TcpClient clientSocket = await listener.AcceptTcpClientAsync();
                IPEndPoint endPoint = (IPEndPoint)clientSocket.Client.LocalEndPoint;
                Console.WriteLine("Server: Connection to {0} established at {1}", endPoint.Address, endPoint.Port);
                ClientHandler2 client = new ClientHandler2(tbxInbox);
                client.StartClient(clientSocket);
            }
        }
    }
    class ClientHandler2
    {
        TcpClient client;
        TextBox tbxLog;

        public ClientHandler2(TextBox tbx)
        {
            tbxLog = tbx;
        }
        public void StartClient(TcpClient inClient)
        {
            this.client = inClient;
            Thread thread = new Thread(Stream);
            thread.Start();
        }
        private void Stream()
        {
            //while (true)
            {
                string message;
                using (var r = new BinaryReader(client.GetStream(), Encoding.UTF8, true))
                {
                    message = r.ReadString();
                    Console.WriteLine("Server: Message read");
                }

                LogMessage(message);
                Console.WriteLine("Server: Message logged");

                using (var w = new BinaryWriter(client.GetStream(), Encoding.UTF8, true))
                {
                    w.Write(message);
                    Console.WriteLine("Server: Message written to stream");
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

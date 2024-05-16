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
    public partial class Server : Form
    {
        TcpListener listener;
        int port = 12345;

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
            btnStartServer.Click += btnStartServer_Click;
            btnStartServer.Show();
        }

        private void CreateTextBoxDelegate(object sender, EventArgs e)
        {
            /*
            // tbxAdress
            this.Controls.Add(tbxAddress);
            // Set properties of tbxAdress
            tbxAddress.Text = "127.0.0.1";
            //tbxAddress.Location = new Point(150, 25);
            tbxAddress.Show();
            */
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

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
            }
            catch (Exception error) { MessageBox.Show(error.Message, Text); return; }

            this.btnStartServer.Enabled = false;
            Recieve();

        }

        private async void Recieve()
        {
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
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

                    // Skicka meddelandet till alla anslutna klienter om du vill
                    // SkickaTillAllaKlienter(meddelande);
                }
            }
            catch (Exception error) { MessageBox.Show(error.Message, Text); }

            c.Close();
        }
        private void LogMessage(string message)
        {
            if (tbxInbox.InvokeRequired)
            {
                tbxInbox.Invoke(new MethodInvoker(delegate { tbxInbox.AppendText($"[{DateTime.Now}] - " + message + Environment.NewLine); }));
            }
            else
            {
                tbxInbox.AppendText($"[{DateTime.Now}] - " + message + Environment.NewLine);
            }
        }
        public async void StartRead(TcpClient c)
        {
            byte[] buffert = new byte[1024];
            int n = 0;
            try
            {
                n = await c.GetStream().ReadAsync(buffert, 0, buffert.Length);
            }
            catch (Exception error) { MessageBox.Show(error.Message, Text); return; }

            tbxInbox.AppendText(Encoding.Unicode.GetString(buffert, 0, n));

            StartRead(c);
        }

    }
}

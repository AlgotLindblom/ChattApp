using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChattApp
{
    public partial class ChattAppLauncher : Form
    {
        Button btnOpenServer = new Button();
        Button btnOpenClient = new Button();

        public ChattAppLauncher()
        {
            InitializeComponent();
            this.Shown += CreateMembersDelegate;
            this.Width = 170;
            this.Height = 200;
        }
        private void CreateMembersDelegate(object sender, EventArgs e)
        {
            // Create btnOpenServer
            
            this.Controls.Add(btnOpenServer);
            // Set properties of btnOpenServer
            btnOpenServer.Text = "Start Server";
            btnOpenServer.Width = 100;
            btnOpenServer.Height = 40;
            btnOpenServer.Location = new Point(25, 25);
            // Set Event properties of btnOpenServer
            btnOpenServer.Click += btnOpenServer_OnClick;
            btnOpenServer.Show();


            this.Controls.Add(btnOpenClient);
            btnOpenClient.Text = "Start Client";
            btnOpenClient.Width = 100;
            btnOpenClient.Height = 40;
            btnOpenClient.Location = new Point(25, 90);
            btnOpenClient.Click += btnOpenClient_Click;
            btnOpenClient.Show();
        }
        private void btnOpenServer_OnClick(object sender, EventArgs e)
        {
            Server Server = new Server();
            Server.Show();
        }
        private void btnOpenClient_Click(object sender, EventArgs e)
        {
            Client Client = new Client();
            Client.Show();
        }
    }
}

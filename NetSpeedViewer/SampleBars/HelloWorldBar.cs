using System;
using System.Windows.Forms;
using BandObjectLib;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace SampleBars
{
    [Guid("D738ECB9-36D4-4E33-B516-909F26995B9E")]
    [BandObject("NetSpeedViewer", BandObjectStyle.Horizontal | BandObjectStyle.ExplorerToolbar | BandObjectStyle.TaskbarToolBar, HelpText = "Shows your current network traffic.")]
    public class HelloWorldBar : BandObject
    {
        private System.ComponentModel.Container components = null;
        private TableLayoutPanel tableLayoutPanel2;
        private Label uploadLabel;
        private Label downloadLabel;
        private Label uploadValLabel;
        private Label downloadValLabel;
        private Timer updateTimer;

        double bytesSentSpeedPrev = 0;
        double bytesReceivedSpeedPrev = 0;

        private NetworkInterface[] nicArr;
        private const int TIMERUDPATE = 1000;
        private int networkInterfaceId = 0;

        public HelloWorldBar()
        {
            InitializeComponent();
            InitializeNetworkInterface();
            InitializeTimer();
        }

        private void InitializeNetworkInterface()
        {
            // Grab all local interfaces to this computer
            nicArr = NetworkInterface.GetAllNetworkInterfaces();

            // Add each interface name to the combo box
            //for (int i = 0; i < nicArr.Length; i++)
                //cmbInterface.Items.Add(nicArr[i].Name);

            // Change the initial selection to the first interface
            //cmbInterface.SelectedIndex = 0;
        }

        private void InitializeTimer()
        {
            updateTimer = new Timer();
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Interval = TIMERUDPATE;
            updateTimer.Start();
        }


        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                downloadValLabel.Text = "nope";
                downloadValLabel.Update();
                return;
            }

            NetworkInterface nic = nicArr[networkInterfaceId];
            IPv4InterfaceStatistics interfaceStats = nic.GetIPv4Statistics();
            double bytesSentSpeed = interfaceStats.BytesSent;
            double bytesReceivedSpeed = interfaceStats.BytesReceived;
            double bytesSentSpeedDelta = bytesSentSpeed - bytesSentSpeedPrev;
            double bytesReceivedSpeedDelta = bytesReceivedSpeed - bytesReceivedSpeedPrev;

            downloadValLabel.Text = $"{string.Format("{0:N2}", (bytesReceivedSpeedDelta / 1024))} kB/s";
            downloadValLabel.Update();
            uploadValLabel.Text = $"{string.Format("{0:N2}", (bytesSentSpeedDelta / 1024))} kB/s";
            uploadValLabel.Update();

            bytesReceivedSpeedPrev = bytesReceivedSpeed;
            bytesSentSpeedPrev = bytesSentSpeed;
        }

        private async void GetTraffic()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                downloadValLabel.Text = "nope";
                downloadValLabel.Update();
                return;
            }

            NetworkInterface[] interfaces
                = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface ni in interfaces)
            {
                var sent = ni.GetIPv4Statistics().BytesSent.ToString();
                var received = ni.GetIPv4Statistics().BytesSent.ToString();
                uploadValLabel.Text = ni.Name;
                uploadValLabel.Update();
                downloadValLabel.Text = ni.Description;
                downloadValLabel.Update();
                await Task.Delay(1000);
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void AddContextMenuAndItems()
        {
            ContextMenu mnuContextMenu = new ContextMenu();
            ContextMenu = mnuContextMenu;

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                downloadValLabel.Text = "nope";
                downloadValLabel.Update();
            }

            NetworkInterface[] interfaces
                = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface ni in interfaces)
            {
                if (!ni.Name.Contains("Loopback"))
                {
                    MenuItem mnuItemNew = new MenuItem();
                    mnuItemNew.Text = $"{ni.Description} ({ni.Name})";
                    mnuContextMenu.MenuItems.Add(mnuItemNew);
                }
            }
        }

        #region Component Designer generated code
        private void InitializeComponent(string t = "test1")
        {
            tableLayoutPanel2 = new TableLayoutPanel();
            uploadLabel = new Label();
            downloadLabel = new Label();
            uploadValLabel = new Label();
            downloadValLabel = new Label();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78F));
            tableLayoutPanel2.Controls.Add(uploadLabel, 0, 0);
            tableLayoutPanel2.Controls.Add(downloadLabel, 0, 1);
            tableLayoutPanel2.Controls.Add(uploadValLabel, 1, 0);
            tableLayoutPanel2.Controls.Add(downloadValLabel, 1, 1);
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(100, 30);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // uploadLabel
            // 
            uploadLabel.AutoSize = true;
            uploadLabel.ForeColor = SystemColors.ControlLightLight;
            uploadLabel.Name = "uploadLabel";
            uploadLabel.TabIndex = 0;
            uploadLabel.Text = "U:";
            uploadLabel.BackColor = Color.Black;

            // 
            // downloadLabel
            // 
            downloadLabel.AutoSize = true;
            downloadLabel.ForeColor = SystemColors.ControlLightLight;
            downloadLabel.Name = "downloadLabel";
            downloadLabel.TabIndex = 1;
            downloadLabel.Text = "D:";
            downloadLabel.BackColor = Color.Black;
            // 
            // uploadValLabel
            // 
            uploadValLabel.AutoSize = false;
            uploadValLabel.Dock = DockStyle.Right;
            uploadValLabel.ForeColor = SystemColors.ControlLightLight;
            uploadValLabel.Name = "uploadValLabel";
            uploadValLabel.TabIndex = 2;
            uploadValLabel.Text = "-- kB/s";
            uploadValLabel.TextAlign = ContentAlignment.MiddleRight;
            uploadLabel.BackColor = Color.Black;
            // 
            // downloadValLabel
            // 
            downloadValLabel.AutoSize = false;
            downloadValLabel.Dock = DockStyle.Right;
            downloadValLabel.ForeColor = SystemColors.ControlLightLight;
            downloadValLabel.Name = "downloadValLabel";
            downloadValLabel.TabIndex = 3;
            downloadValLabel.Text = "-- kB/s";
            downloadValLabel.TextAlign = ContentAlignment.MiddleRight;
            downloadValLabel.BackColor = Color.Black;
            // 
            // HelloWorldBar
            // 
            Controls.Add(tableLayoutPanel2);
            MinSize = new Size(100, 30);
            Size = new Size(100, 30);
            Name = "NetSpeedViewerBar";
            tableLayoutPanel2.BackColor = Color.Black;
            BackColor = Color.Transparent;

            AddContextMenuAndItems();


            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);

        }
        #endregion

    }
}

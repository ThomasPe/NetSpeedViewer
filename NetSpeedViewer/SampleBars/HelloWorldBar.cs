using System;
using System.Windows.Forms;
using BandObjectLib;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Collections.Generic;

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
        private MenuItem checkedMenuItem;

        private bool isWatchTrafficEnabled = true;
        private Dictionary<string, double> formats = new Dictionary<string, double>(){
            { "kbit", (1000 / 8) },
            { "kB", 1000 },
            { "Mbit", ((1000 * 1000) / 8) },
            { "MB", (1000 * 1000) }
        };
        private string formatText = "kB";
        private double formatVal = 1000;

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
            if (!isWatchTrafficEnabled)
            {
                updateTimer.Stop();
                return;
            }
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("There's been a network error.");
                return;
            }

            NetworkInterface nic = nicArr[networkInterfaceId];
            IPv4InterfaceStatistics interfaceStats = nic.GetIPv4Statistics();
            double bytesSentSpeed = interfaceStats.BytesSent;
            double bytesReceivedSpeed = interfaceStats.BytesReceived;
            double bytesSentSpeedDelta = bytesSentSpeed - bytesSentSpeedPrev;
            double bytesReceivedSpeedDelta = bytesReceivedSpeed - bytesReceivedSpeedPrev;


            // don't update text if it's the first cycle
            if(bytesReceivedSpeedPrev != 0 && bytesSentSpeedPrev != 0)
            {
                downloadValLabel.Text = $"{string.Format("{0:N2}", (bytesReceivedSpeedDelta / formatVal))} {formatText}/s";
                downloadValLabel.Update();
                uploadValLabel.Text = $"{string.Format("{0:N2}", (bytesSentSpeedDelta / formatVal))} {formatText}/s";
                uploadValLabel.Update();
            }

            bytesReceivedSpeedPrev = bytesReceivedSpeed;
            bytesSentSpeedPrev = bytesSentSpeed;
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
            ContextMenu = new ContextMenu();

            // On / Off
            var onoffmnuItem = new MenuItem();
            onoffmnuItem.Click += ToggleWatchTraffic;
            onoffmnuItem.Text = "Watch traffic";
            onoffmnuItem.Checked = isWatchTrafficEnabled;
            ContextMenu.MenuItems.Add(onoffmnuItem);

            ContextMenu.MenuItems.Add("-");

            // Select Network

            NetworkInterface[] interfaces
                = NetworkInterface.GetAllNetworkInterfaces();

            for(int i = 0; i < interfaces.Length; i++)
            {
                NetworkInterface ni = interfaces[i];
                if (!ni.Name.Contains("Loopback"))
                {
                    MenuItem mnuItemNew = new MenuItem();
                    mnuItemNew.Text = $"{ni.Description} ({ni.Name})";
                    mnuItemNew.Tag = i;
                    mnuItemNew.RadioCheck = true;
                    mnuItemNew.Click += SelectNetwork;
                    if(i == 0)
                    {
                        checkedMenuItem = mnuItemNew;
                        mnuItemNew.Checked = true;
                    }
                    ContextMenu.MenuItems.Add(mnuItemNew);
                }
            }

            ContextMenu.MenuItems.Add("-");

            // Traffic Format
            foreach(var format in formats)
            {
                var fmi = new MenuItem();
                fmi.Text = format.Key;
                fmi.Tag = format.Value;
                fmi.RadioCheck = true;
                fmi.Click += ChangeFormat_Click;
                ContextMenu.MenuItems.Add(fmi);
            }

        }

        private void ChangeFormat_Click(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            formatText = item.Text;
            formatVal = (double)item.Tag;
        }

        private void ToggleWatchTraffic(object sender, EventArgs e)
        {
            isWatchTrafficEnabled = !isWatchTrafficEnabled;
            ((MenuItem)sender).Checked = isWatchTrafficEnabled;
            if (isWatchTrafficEnabled)
            {
                updateTimer.Start();
            } else
            {
                updateTimer.Stop();
                bytesReceivedSpeedPrev = 0;
                bytesSentSpeedPrev = 0;
                downloadValLabel.Text = $"--- {formatText}/s";
                downloadValLabel.Update();
                uploadValLabel.Text = $"--- {formatText}/s";
                uploadValLabel.Update();
            }
        }

        private void SelectNetwork(object sender, EventArgs e)
        {
            checkedMenuItem.Checked = false;
            checkedMenuItem = (MenuItem)sender;
            checkedMenuItem.Checked = true;
            networkInterfaceId = (int)checkedMenuItem.Tag;
            bytesReceivedSpeedPrev = 0;
            bytesSentSpeedPrev = 0;
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
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel2.Controls.Add(uploadLabel, 0, 0);
            tableLayoutPanel2.Controls.Add(downloadLabel, 0, 1);
            tableLayoutPanel2.Controls.Add(uploadValLabel, 1, 0);
            tableLayoutPanel2.Controls.Add(downloadValLabel, 1, 1);
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(120, 30);
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
            uploadValLabel.Text = $"--- {formatText}/s";
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
            downloadValLabel.Text = $"--- {formatText}/s";
            downloadValLabel.TextAlign = ContentAlignment.MiddleRight;
            downloadValLabel.BackColor = Color.Black;
            // 
            // HelloWorldBar
            // 
            Controls.Add(tableLayoutPanel2);
            MinSize = new Size(120, 30);
            Size = new Size(120, 30);
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

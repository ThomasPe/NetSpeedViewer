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
    [BandObject("NetSpeedViewer Bar", BandObjectStyle.Horizontal | BandObjectStyle.ExplorerToolbar | BandObjectStyle.TaskbarToolBar, HelpText = "Shows your current network traffic.")]
    public class HelloWorldBar : BandObject
    {
        private System.ComponentModel.Container components = null;
        private TableLayoutPanel tableLayoutPanel2;
        private Label uploadLabel;
        private Label downloadLabel;
        private Label uploadValLabel;
        private Label downloadValLabel;
        private Timer updateTimer;

        public HelloWorldBar()
        {
            InitializeComponent();
            updateTimer = new Timer();
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Interval = 500;
            updateTimer.Start();
            //GetTraffic();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                downloadValLabel.Text = "nope";
                downloadValLabel.Update();
                return;
            }

            NetworkInterface[] interfaces
                = NetworkInterface.GetAllNetworkInterfaces();
            var down = interfaces[0].GetIPStatistics().BytesReceived / 1024;
            downloadValLabel.Text = down.ToString();
            downloadValLabel.Update();
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
            this.ContextMenu = mnuContextMenu;

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
                    mnuItemNew.Text = $"{ni.Name} | {ni.Description} | {ni.Speed}";
                    mnuContextMenu.MenuItems.Add(mnuItemNew);
                }
            }
        }

        #region Component Designer generated code
        private void InitializeComponent(string t = "test1")
        {
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.uploadLabel = new System.Windows.Forms.Label();
            this.downloadLabel = new System.Windows.Forms.Label();
            this.uploadValLabel = new System.Windows.Forms.Label();
            this.downloadValLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.Controls.Add(this.uploadLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.downloadLabel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.uploadValLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.downloadValLabel, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(192, 30);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // uploadLabel
            // 
            this.uploadLabel.AutoSize = true;
            this.uploadLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            //this.uploadLabel.Location = new System.Drawing.Point(3, 0);
            this.uploadLabel.Name = "uploadLabel";
            //this.uploadLabel.Size = new System.Drawing.Size(18, 13);
            this.uploadLabel.TabIndex = 0;
            this.uploadLabel.Text = "U:";
            uploadLabel.BackColor = Color.Black;

            // 
            // downloadLabel
            // 
            this.downloadLabel.AutoSize = true;
            this.downloadLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            //this.downloadLabel.Location = new System.Drawing.Point(3, 33);
            this.downloadLabel.Name = "downloadLabel";
            //this.downloadLabel.Size = new System.Drawing.Size(18, 13);
            this.downloadLabel.TabIndex = 1;
            this.downloadLabel.Text = "D:";
            downloadLabel.BackColor = Color.Black;
            // 
            // uploadValLabel
            // 
            this.uploadValLabel.AutoSize = true;
            this.uploadValLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            //this.uploadValLabel.Location = new System.Drawing.Point(49, 0);
            this.uploadValLabel.Name = "uploadValLabel";
            //this.uploadValLabel.Size = new System.Drawing.Size(68, 13);
            this.uploadValLabel.TabIndex = 2;
            this.uploadValLabel.Text = "111";
            uploadLabel.BackColor = Color.Black;
            // 
            // downloadValLabel
            // 
            this.downloadValLabel.AutoSize = true;
            this.downloadValLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            //this.downloadValLabel.Location = new System.Drawing.Point(49, 33);
            this.downloadValLabel.Name = "downloadValLabel";
            //this.downloadValLabel.Size = new System.Drawing.Size(82, 13);
            this.downloadValLabel.TabIndex = 3;
            this.downloadValLabel.Text = "000";
            downloadValLabel.BackColor = Color.Black;
            // 
            // HelloWorldBar
            // 
            this.Controls.Add(this.tableLayoutPanel2);
            this.MinSize = new System.Drawing.Size(192, 30);
            this.Size = new System.Drawing.Size(192, 30);
            this.Name = "HelloWorldBar";
            tableLayoutPanel2.BackColor = Color.Black;
            this.BackColor = Color.Transparent;

            AddContextMenuAndItems();


            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

    }
}

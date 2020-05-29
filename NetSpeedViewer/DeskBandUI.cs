using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace NetSpeedViewer
{
    public partial class DeskBandUI : UserControl
    {
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

        public DeskBandUI()
        {
            InitializeComponent();
            InitializeNetworkInterface();
            InitializeTimer();
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

            for (int i = 0; i < interfaces.Length; i++)
            {
                NetworkInterface ni = interfaces[i];
                if (!ni.Name.Contains("Loopback"))
                {
                    MenuItem mnuItemNew = new MenuItem();
                    mnuItemNew.Text = $"{ni.Description} ({ni.Name})";
                    mnuItemNew.Tag = i;
                    mnuItemNew.RadioCheck = true;
                    mnuItemNew.Click += SelectNetwork;
                    if (i == 0)
                    {
                        checkedMenuItem = mnuItemNew;
                        mnuItemNew.Checked = true;
                    }
                    ContextMenu.MenuItems.Add(mnuItemNew);
                }
            }

            ContextMenu.MenuItems.Add("-");

            // Traffic Format
            foreach (var format in formats)
            {
                var fmi = new MenuItem();
                fmi.Text = format.Key;
                fmi.Tag = format.Value;
                fmi.RadioCheck = true;
                fmi.Click += ChangeFormat_Click;
                ContextMenu.MenuItems.Add(fmi);
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
            }
            else
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
            if (bytesReceivedSpeedPrev != 0 && bytesSentSpeedPrev != 0)
            {
                downloadValLabel.Text = $"{string.Format("{0:N2}", (bytesReceivedSpeedDelta / formatVal))} {formatText}/s";
                downloadValLabel.Update();
                uploadValLabel.Text = $"{string.Format("{0:N2}", (bytesSentSpeedDelta / formatVal))} {formatText}/s";
                uploadValLabel.Update();
            }

            bytesReceivedSpeedPrev = bytesReceivedSpeed;
            bytesSentSpeedPrev = bytesSentSpeed;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

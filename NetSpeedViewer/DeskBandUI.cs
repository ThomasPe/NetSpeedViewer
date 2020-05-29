using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Linq;
using System.Configuration;
using System.Diagnostics;

namespace NetSpeedViewer
{
    public partial class DeskBandUI : UserControl
    {
        private Timer updateTimer;

        double bytesSentSpeedPrev = 0;
        double bytesReceivedSpeedPrev = 0;

        private NetworkInterface[] nicArr;
        private const int TIMERUDPATE = 1000;
        private int selectedNetworkInterface = 0;
        private MenuItem checkedMenuItem;

        private bool isWatchTrafficEnabled = true;
        private readonly Dictionary<string, double> formats = new Dictionary<string, double>(){
            { "kbit", 1000 / 8 },
            { "kB", 1000 },
            { "Mbit", 1000 * 1000 / 8 },
            { "MB", 1000 * 1000 }
        };
        private string formatText = "kB";
        private double formatVal = 1000;

        public DeskBandUI()
        {
            InitializeNetworkInterface();
            InitializeComponent();
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
                    if (i == selectedNetworkInterface)
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
                var fmi = new MenuItem
                {
                    Text = format.Key,
                    Tag = format.Value,
                    RadioCheck = true
                };
                fmi.Click += ChangeFormat_Click;
                ContextMenu.MenuItems.Add(fmi);
            }

        }

        private void SelectNetwork(object sender, EventArgs e)
        {
            checkedMenuItem.Checked = false;
            checkedMenuItem = (MenuItem)sender;
            checkedMenuItem.Checked = true;
            selectedNetworkInterface = (int)checkedMenuItem.Tag;

            ToggleWatchTraffic(null, null);
            ToggleWatchTraffic(null, null);

            Settings.Default.Interface = (int)checkedMenuItem.Tag;
            Settings.Default.Save();
            Settings.Default.Reload();
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            //MessageBox.Show($"Local user config path: {config.FilePath}");
            //Process.Start(config.FilePath.Replace("user.config", ""));



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
            if(sender != null)
            {
                ((MenuItem)sender).Checked = isWatchTrafficEnabled;
            }
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
            selectedNetworkInterface = Settings.Default.Interface;
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

            NetworkInterface nic = nicArr[0];
            if(selectedNetworkInterface < nicArr.Length)
            {
                nic = nicArr[selectedNetworkInterface];
            }

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

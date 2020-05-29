using SharpShell.Attributes;
using SharpShell.SharpDeskBand;
using System.Runtime.InteropServices;

namespace NetSpeedViewer
{
    [ComVisible(true)]
    [DisplayName("NetSpeedViewer")]
    public class NetSpeedViewerDeskBand : SharpDeskBand
    {
        protected override System.Windows.Forms.UserControl CreateDeskBand()
        {
            return new DeskBandUI();
        }

        protected override BandOptions GetBandOptions()
        {
            return new BandOptions
            {
                HasVariableHeight = false,
                IsSunken = false,
                ShowTitle = true,
                Title = "NetSpeedViewer",
                UseBackgroundColour = false,
                AlwaysShowGripper = true
            };
        }
    }
}

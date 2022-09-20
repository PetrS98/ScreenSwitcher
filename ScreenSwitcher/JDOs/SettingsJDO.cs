using ScreenSwitcher.UDTs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSwitcher.JDOs
{
    public class SettingsJDO
    {
        public short[] MonitorsOrder { get; set; } = { 0, 0 };
        public int MonitorResolutionIndex { get; set; } = 0;
        public string MonitorResolution { get; set; } = "1920x1080";
        public int ChangeInterval { get; set; } = 30000;
        public List<DataGridApplicationUDT> ApplicationInformation { get; set; } = new List<DataGridApplicationUDT> ();
    }
}

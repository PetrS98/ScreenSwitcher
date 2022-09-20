using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSwitcher.UDTs
{
    public class DataGridApplicationUDT
    {
        public string? Name { get; set; }
        public short? OffsetTop { get; set; }
        public short? OffsetLeft { get; set; }
        public short? OffsetBot { get; set; }
        public short? OffsetRight { get; set; }
        public bool Used { get; set; }
    }
}

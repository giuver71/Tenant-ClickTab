using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.HelperClass
{
    public class NavItem
    {
        public string name { get; set; }
        public string url { get; set; }
        public string icon { get; set; }

        public List<NavItem> children { get; set; }
    }
}

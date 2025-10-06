using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Mappings.ModelsDTO.Charts
{
    public class XAxisConfig
    {
        public string type { get { return "category"; } }
        public string[] data { get; set; }
    }
}

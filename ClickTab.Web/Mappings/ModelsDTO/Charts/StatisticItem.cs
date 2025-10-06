using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Mappings.ModelsDTO.Charts
{
    public class StatisticItem
    {
        public string Icon { get; set; }
        public bool IsFontAwesomeIcon { get; set; }
        public string IconClass { get; set; }
        public string Label { get; set; }
        public string LabelClass { get; set; }
        public string ProgressBarColor { get; set; }
        public decimal Value { get; set; }
        public decimal? MaxValue { get; set; }
        public string Percentage { get; set; }
    }
}

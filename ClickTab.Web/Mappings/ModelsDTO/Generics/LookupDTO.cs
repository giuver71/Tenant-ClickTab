using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Mappings.ModelsDTO.Generics
{
    /// <summary>
    /// Oggetto rimappato da tutte le lookup.
    /// </summary>
    public class LookupDTO
    {
        public object ID { get; set; }
        public string Label { get; set; }
        public object FullObject { get; set; }
    }
}

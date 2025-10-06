using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Mappings
{
    public class CsvImportResultDTO
    {
        public bool HasErrors { get; set; }
        public string FileBase64 { get; set; }
        public string FileContentType { get; set; }
    }
}

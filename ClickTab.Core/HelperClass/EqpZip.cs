using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.HelperClass
{
    public class EqpZip
    {
        public string Name { get; set; }
        public string ContentType { get; set; } = "application/zip";
        public byte[] Zip { get; set; }
    }
}

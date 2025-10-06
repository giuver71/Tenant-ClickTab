using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Models
{
    public class ErrorResponseObject
    {
        public string message { get; set; }
        public string stack { get; set; }
    }
}

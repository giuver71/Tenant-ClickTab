using ClickTab.Core.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.Tenant
{
    public class TenantInfo
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public DbProvider Provider { get; set; }
    }
}

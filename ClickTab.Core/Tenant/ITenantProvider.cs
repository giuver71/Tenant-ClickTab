using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.Tenant
{
    public interface ITenantProvider
    {
        TenantInfo CurrentTenant { get; }
        void SetTenant(string tenantName);
    }
}

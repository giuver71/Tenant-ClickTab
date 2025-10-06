using ClickTab.Core.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.Services
{
    public interface ITenantConnectionService
    {
        string? GetConnectionString(string tenant);
        DbProvider GetDbProvider(string tenant);
        IEnumerable<string> GetAllTenantIds();

    }
}

using ClickTab.Core.HelperService;
using ClickTab.Core.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.Tenant
{
    public class TenantConnectionService : ITenantConnectionService
    {
        private readonly IDictionary<string, string> _connectionStrings;
        private readonly IDictionary<string, DbProvider> _dbProviders;

        private readonly string _defaultTenant;

        public TenantConnectionService(IConfiguration config)
        {
            _defaultTenant = config.GetSection("MultiTenant")["DefaultTenant"];

            _connectionStrings = config.GetSection("ConnectionStrings")
                                       .Get<Dictionary<string, string>>();

            var dbProvidersRaw = config.GetSection("DbProviders")
                                       .Get<Dictionary<string, string>>();

            _dbProviders = dbProvidersRaw.ToDictionary(
                kv => kv.Key,
                kv => Enum.Parse<DbProvider>(kv.Value, ignoreCase: true)
            );
        }

        public string GetConnectionString(string tenant)
        {
            tenant = string.IsNullOrWhiteSpace(tenant) ? _defaultTenant : tenant;

            if (!_connectionStrings.ContainsKey(tenant))
                throw new UnauthorizedAccessException($"Tenant non autorizzato: {tenant}");

            return _connectionStrings[tenant];
        }

        public DbProvider GetDbProvider(string tenant)
        {
            tenant = string.IsNullOrWhiteSpace(tenant) ? _defaultTenant : tenant;

            if (!_dbProviders.ContainsKey(tenant))
                throw new UnauthorizedAccessException($"Provider mancante per tenant: {tenant}");

            return _dbProviders[tenant];
        }
        public IEnumerable<string> GetAllTenantIds()
        {
            return _connectionStrings.Keys;
        }


    }
}

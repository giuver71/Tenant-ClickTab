using ClickTab.Core.HelperService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.Tenant
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private TenantInfo _tenantInfo;

        public TenantProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public TenantInfo CurrentTenant => _tenantInfo;

        public void SetTenant(string tenantName)
        {
            var section = _configuration.GetSection($"Tenants:{tenantName}");
            if (!section.Exists())
                throw new Exception($"Tenant '{tenantName}' non trovato.");

            _tenantInfo = new TenantInfo
            {
                Name = tenantName,
                ConnectionString = section["ConnectionString"],
                Provider = (DbProvider)int.Parse(section["Provider"])
            };
        }
    }
}

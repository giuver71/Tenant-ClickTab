using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? tenant = null;

            // 1. Prova a leggere da query string
            if (context.Request.Query.TryGetValue("tenant", out var tenantQuery) && !string.IsNullOrWhiteSpace(tenantQuery))
            {
                tenant = tenantQuery.ToString();
            }

            // 2. Se non c'è, prova a leggere dall'header X-Tenant-ID
            else if (context.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantHeader) && !string.IsNullOrWhiteSpace(tenantHeader))
            {
                tenant = tenantHeader.ToString();
            }

            // 3. Se trovato, salvalo nel contesto per questa richiesta
            if (!string.IsNullOrWhiteSpace(tenant))
            {
                context.Items["Tenant"] = tenant;
            }

            await _next(context);
        }
    }
}

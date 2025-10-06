using ClickTab.Core.Tenant;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ClickTab.Web
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            var tenant = context.Request.Query["tenant"].ToString();
            if (string.IsNullOrEmpty(tenant))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Tenant mancante.");
                return;
            }

            tenantProvider.SetTenant(tenant);
            await _next(context);
        }
    }
}

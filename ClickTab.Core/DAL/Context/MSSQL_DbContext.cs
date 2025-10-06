using ClickTab.Core.HelperService;
using ClickTab.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.DAL.Context
{
    //public class MSSQL_DbContext : DatabaseContext
    //{
    //    public MSSQL_DbContext() : base(DbProvider.SQLServer) { }

    //    public MSSQL_DbContext(DbContextOptions options) : base(options) { }

    //    public MSSQL_DbContext(string ConnectionString, DbProvider Provider) : base(ConnectionString, Provider) { }

    //}
    public class MSSQL_DbContext : DatabaseContext
    {
        public MSSQL_DbContext(string connectionString, DbProvider provider)
    : base(connectionString, provider)
        { }

        public MSSQL_DbContext(IHttpContextAccessor httpContextAccessor,
                               ITenantConnectionService tenantConnectionService)
            : base(
                  tenantConnectionService.GetConnectionString(
                      httpContextAccessor.HttpContext?.Items["Tenant"]?.ToString() ?? string.Empty
                  ),
                  tenantConnectionService.GetDbProvider(
                      httpContextAccessor.HttpContext?.Items["Tenant"]?.ToString() ?? string.Empty
                  ))
        { }
    }
}

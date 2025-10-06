using ClickTab.Core.HelperService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.DAL.Context
{
    public class MySQL_DbContext : DatabaseContext
    {
        public MySQL_DbContext() : base(DbProvider.MySql) { }
        public MySQL_DbContext(DbContextOptions options) : base(options) { }
        public MySQL_DbContext(string ConnectionString, DbProvider Provider) : base(ConnectionString, Provider) { }
    }
}

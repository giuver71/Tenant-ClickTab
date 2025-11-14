using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Registry;
using ClickTab.Core.DAL.Models.Tables;
using EQP.EFRepository.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Repository.Registry
{
    public class DistributorRepository : IdentityRepository<Distributor>
    {
        private DatabaseContext _ctx;
        public DistributorRepository(DatabaseContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }
    }
}

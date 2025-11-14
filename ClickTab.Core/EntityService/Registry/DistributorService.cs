using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Registry;
using ClickTab.Core.DAL.Models.Tables;
using ClickTab.Core.DAL.Repository.Generics;
using ClickTab.Core.DAL.Repository.Registry;
using EQP.EFRepository.Core.DAL;
using EQP.EFRepository.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.EntityService.Registry
{
    
    public class DistributorService : IdentityService<DistributorRepository, Distributor>
    {
        private DatabaseContext _ctx;
        public DistributorService(UnitOfWork<DatabaseContext> uow, DatabaseContext ctx) : base(uow)
        {
            _ctx = ctx;
        }




    }

}

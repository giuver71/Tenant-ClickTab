using EQP.EFRepository.Core.Repository;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClickTab.Core.DAL.Models.Tables;

namespace ClickTab.Core.DAL.Repository.Generics
{
    public class VatRepository : IdentityRepository<Vat>
    {
        private DatabaseContext _ctx;
        public VatRepository(DatabaseContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }
    }
}

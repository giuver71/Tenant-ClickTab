using EQP.EFRepository.Core.Repository;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Repository.Generics
{
    public class RuleRepository : IdentityRepository<Rule>
    {
        private DatabaseContext _context;
        public RuleRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
            _context = databaseContext;
        }
    }
}

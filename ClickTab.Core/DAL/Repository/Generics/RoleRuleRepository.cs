using EQP.EFRepository.Core.Repository;
using Microsoft.EntityFrameworkCore;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Repository.Generics
{
    public class RoleRuleRepository : IdentityRepository<RoleRule>
    {
        private DatabaseContext _context;
        public RoleRuleRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
            _context = databaseContext;
        }


       

       
    }
}

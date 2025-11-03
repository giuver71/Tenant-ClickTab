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
    public class CategoryRepository : IdentityRepository<Category>
    {
        private DatabaseContext _context;
        public CategoryRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
            _context = databaseContext;
        }
    }
}

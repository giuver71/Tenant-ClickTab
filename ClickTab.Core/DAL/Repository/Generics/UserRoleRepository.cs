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
    public class UserRoleRepository : IdentityRepository<UserRole>
    {
        public UserRoleRepository(DatabaseContext context) : base(context) { }
    }
}

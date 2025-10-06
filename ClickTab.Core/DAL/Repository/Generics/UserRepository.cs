using EQP.EFRepository.Core.Repository;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models;
using ClickTab.Core.DAL.Models.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ClickTab.Core.DAL.Repository.Generics
{
    public class UserRepository : IdentityRepository<User>
    {
        private DatabaseContext _DatabaseContext;
        public UserRepository(DatabaseContext databaseContext) : base(databaseContext) 
        {
            _DatabaseContext = databaseContext;
        }

        public override void Save(User entity, bool checkConcurrency = true)
        {
            // <ewz:manageRelationships>
            base.Save(entity);
        }

        // <ewz:Include>

        public User GetFull(int id)
        {
            IQueryable<User> _data = _DatabaseContext.Users.Where(a => a.ID == id);

            return _data.FirstOrDefault();
        }

        public List<User> GetAllFull()
        {
            IQueryable<User> _data = _DatabaseContext.Users;

            return _data.ToList();
        }
    }
}

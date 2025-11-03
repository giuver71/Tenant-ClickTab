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
    public class RoleRepository : IdentityRepository<Role>
    {
        private DatabaseContext _context;
        public RoleRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
            _context = databaseContext;
        }


        private IQueryable<Role> GetRoleQuery(int ID, params Expression<Func<Role, object>>[] includes)
        {
            IQueryable<Role> query = _context.Roles.Where(e => e.ID == ID)
                                                      .Include(e => e.RoleRules);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query;
        }

        public Role GetFullRole(int id)
        {
            IQueryable<Role> roles = _context.Roles.Where(p => p.ID == id );
            //roles = roles.Include(lu => lu.MenuRole).ThenInclude(p => p.Menu);
            roles = roles.Include(lu => lu.RoleRules).ThenInclude(p => p.Rule);
            return roles.FirstOrDefault();
        }

        public override void Save(Role entity, bool checkConcurrency = true)
        {
            ManageOneToManyRelationEntityState(entity.RoleRules, p => p.FK_Role == entity.ID);

            base.Save(entity);
        }

        public Role GetFullRoleAsNoTracking(int id, params Expression<Func<Role, object>>[] includes)
        {
            IQueryable<Role> query = this.GetRoleQuery(id, includes);
            query = query.AsNoTracking();
            Role role = query.FirstOrDefault();

            return role;

        }

        public Role GetFull(int id)
        {
            IQueryable<Role> _data = _context.Roles.Where(a => a.ID == id).Include(p => p.RoleRules).ThenInclude(p=>p.Rule);

            return _data.FirstOrDefault();
        }
    }
}

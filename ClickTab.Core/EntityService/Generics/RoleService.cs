using EQP.EFRepository.Core.DAL;
using EQP.EFRepository.Core.Repository;
using EQP.EFRepository.Core.Services;
using Microsoft.EntityFrameworkCore;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.DAL.Repository.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.EntityService.Generics
{
    public class RoleService : IdentityService<RoleRepository, Role>
    {
        private DatabaseContext _dbContext;
        public RoleService(UnitOfWork<DatabaseContext> uow, DatabaseContext dbContext) : base(uow)
        {
            _dbContext = dbContext;
        }

        public Role GetFullRole(int id)
        {
            Role role = _repository.GetFullRole(id);
            return role;
        }

        protected override void SaveValidation(Role entity)
        {
            // Dalla lista di regole gruppo per FK_Rule e seleziono quelle presenti più di una volta. Se la selezione ha successo
            // ci sono duplicati.

            var duplicates = entity.RoleRules.GroupBy(x => x.FK_Rule)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            if (duplicates.Count > 0)
            {
                throw new Exception($"Sono presenti regole duplicate all'interno del ruolo");
            }
            base.SaveValidation(entity);
        }

        public int DuplicateRole(int id, int FK_Facilitie)
        {
            Role newRole = _repository.GetFullRoleAsNoTracking(id);
            newRole.ID = 0;
            newRole.Description = "Duplicated of " + newRole.Description;
            foreach (RoleRule roleRule in newRole.RoleRules)
            {
                roleRule.ID = 0;
                roleRule.FK_Role = 0;

            }

            return Save(newRole);

        }

        public List<RoleRule> GetRulesByRole(int id)
        {
            List<Rule> rules = _dbContext.Rules.ToList();
            Role role=_dbContext.Roles.Where(p=>p.ID == id).FirstOrDefault();
            List<RoleRule> roleRules = _dbContext.RoleRules.Include(p=>p.Role).Include(p=>p.Rule).Where(p => p.FK_Role == id).ToList();
            foreach (Rule rule in rules)
            {
                RoleRule currentRoleRule = roleRules.Find(p => p.FK_Rule == rule.ID);
                if (currentRoleRule==null)
                {
                    RoleRule notAssoaciated = new RoleRule();
                    notAssoaciated.FK_Rule = rule.ID;
                    notAssoaciated.Rule = rule;
                    notAssoaciated.FK_Role = id;
                    notAssoaciated.Role = role;
                    notAssoaciated.IsSelected = false;
                    roleRules.Add(notAssoaciated);
                }
                else
                {
                    currentRoleRule.IsSelected = true;
                }

            }
            return roleRules.OrderByDescending(p=>p.IsSelected).ToList();
        }

        
    }
}

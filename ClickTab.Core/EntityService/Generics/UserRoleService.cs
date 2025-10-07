using EQP.EFRepository.Core.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Core.DAL.Repository.Generics;
using ClickTab.Core.HelperService;
using EQP.EFRepository.Core.Services;

namespace ClickTab.Core.EntityService.Generics
{
    public class UserRoleService : IdentityService<UserRoleRepository, UserRole>
    {
        private DatabaseContext _ctx;
        private SessionService _sessionService;
        public UserRoleService(UnitOfWork<DatabaseContext> uow, DatabaseContext ctx, SessionService sessionService) : base(uow)
        {
            _ctx = ctx;
            _sessionService = sessionService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns>Restituisce la lista di tutti i ruoli utente compresi di regole</returns>
        public List<UserRole> GetFullRoles(int userID)
        {
            IQueryable<UserRole> allRoles = _ctx.UserRoles.Where(x => x.FK_User == userID).Include(x => x.Role).ThenInclude(x => x.RoleRules).ThenInclude(x => x.Rule);
            List<UserRole> userRoles = allRoles.ToList();
            return userRoles;
        }

       

    }
}

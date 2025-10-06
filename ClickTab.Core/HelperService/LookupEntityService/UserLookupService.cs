using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using EQP.EFRepository.Core.Interface;
using EQP.EFRepository.Core.Models;
using EQP.EFRepository.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.HelperService.LookupEntityService
{
    public class UserLookupService : LookupService
    {
        public UserLookupService(DatabaseContext context) : base(context)
        {
        }

        public override List<IBaseEntity> GetEntitiesFromType(List<ComplexLinqPredicate> entityLinqPredicates)
        {
            return base.GetEntities<User>(entityLinqPredicates);
        }
    }
}

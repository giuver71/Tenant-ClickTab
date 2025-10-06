using EQP.EFRepository.Core.Interface;
using EQP.EFRepository.Core.Models;
using EQP.EFRepository.Core.Services;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.HelperService.LookupEntityService.Generics
{
    public class MenuLookupService : LookupService
    {
        public MenuLookupService(DatabaseContext context) : base(context)
        {
        }

        public override List<IBaseEntity> GetEntitiesFromType(List<ComplexLinqPredicate> entityLinqPredicates)
        {
            return base.GetEntities<Role>(entityLinqPredicates);
        }
    }
}

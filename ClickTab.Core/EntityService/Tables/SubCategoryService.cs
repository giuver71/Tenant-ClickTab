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
using ClickTab.Core.DAL.Models.Tables;

namespace ClickTab.Core.EntityService.Generics
{
    public class SubCategoryService : IdentityService<SubCategoryRepository, SubCategory>
    {
        private DatabaseContext _ctx;
        public SubCategoryService(UnitOfWork<DatabaseContext> uow, DatabaseContext ctx) : base(uow)
        {
            _ctx = ctx;
        }


       

    }
}

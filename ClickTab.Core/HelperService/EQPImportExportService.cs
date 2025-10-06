using ClickTab.Core.DAL.Context;
using EQP.EFRepository.Core.DAL;
using EQP.EFRepository.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.HelperService
{
    public class EQPImportExportService : ImportExportService
    {
        public EQPImportExportService(DatabaseContext ctx, UnitOfWork<DatabaseContext> uow) : base(ctx, uow)
        {
        }
    }
}

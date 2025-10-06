using ClickTab.Core.DAL.Context;
using EQP.EFRepository.Core.DAL;
using EQP.EFRepository.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.HelperService
{
    /// <summary>
    /// Servizio che espone le funzionalità (da EFRepository) per leggere db set generici e appiattire i 
    /// dati in un dizionario di valori (utile ad esempio per la stampa dei documenti)
    /// </summary>
    public class EQPDbContextService : DbContextService
    {
        public EQPDbContextService(DatabaseContext ctx, UnitOfWork<DatabaseContext> uow) : base(ctx, uow)
        {
        }
    }
}

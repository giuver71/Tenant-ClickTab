using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.HelperService.XLSUpdateHelper
{

    /// <summary>
    /// Servizio che si ccupa di aggiornare la tabella RULLES tramite l'apposito file Xlsx
    /// L'attributo FileXlsImport serve ad attivare automaticamente la funzione di aggiornamento tramite FileXlsImportProviderFactory
    /// </summary>
    [FileXlsImport("Rules")]
    public  class SyncRulesService :XlsUpdateBase<Rule>
    {
        public override DbSet<Rule> GetDbSet(DatabaseContext ctx) => ctx.Rules;

        public override void Sync(FileService fileService, ConfigurationService configService, DatabaseContext ctx, FileInfo info = null)
        {
            if (info == null)
            {
                string fileName = base.GetFileNameByAttribute();
                info = base.GetFileInfo(fileName);

            }

            if (!base.CheckUpdate(ctx, info))
                return;
            // Disabilito il controllo sui vincoli referenziali
            ctx.Database.ExecuteSqlRaw("Alter Table Rules NOCHECK CONSTRAINT all");
            ctx.Database.ExecuteSqlRaw("Alter Table RoleRules NOCHECK CONSTRAINT all");

            ctx.Database.ExecuteSqlRaw("Delete from Rules");


            base.DefaultSync(ctx, info);

            //Cancello Tutte le roleRules "orfane" di Rule (cio avvienbe se dal foglio excel vengono rimosse delle regole)
            // Solo in local 
            ctx.Database.ExecuteSqlRaw("Delete From RoleRules Where FK_Rule not in (Select ID from Rules)");

            // Riabilito il controllo sui vincoli referenziali
            ctx.Database.ExecuteSqlRaw("Alter Table Rules CHECK CONSTRAINT all");
            ctx.Database.ExecuteSqlRaw("Alter Table RoleRules CHECK CONSTRAINT all");

            base.UpdateEntity(ctx, info);
        }
    }
}

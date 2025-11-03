using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ClickTab.Core.DAL.Models.Generics;

namespace ClickTab.Core.HelperService.XLSUpdateHelper
{
    /// <summary>
    /// Servizio che si ccupa di aggiornare la tabella ROLES tramite l'apposito file Xlsx
    /// L'attributo FileXlsImport serve ad attivare automaticamente la funzione di aggiornamento tramite FileXlsImportProviderFactory
    /// </summary>
    [FileXlsImport("Roles")]
    public class SyncRolesService : XlsUpdateBase<Role>
    {
        public override DbSet<Role> GetDbSet(DatabaseContext ctx)
=> ctx.Roles;

        public override void Sync(FileService fileService, ConfigurationService configService, DatabaseContext ctx, FileInfo info = null)
        {

            if (info == null)
            {
                string fileName = base.GetFileNameByAttribute();
                info = base.GetFileInfo(fileName);

            }

            if (!base.CheckUpdate(ctx, info))
                return;

            base.SyncDependencies(fileService, configService, ctx);


            base.DefaultSync(ctx, info);

            base.UpdateEntity(ctx, info);

        }
    }
}

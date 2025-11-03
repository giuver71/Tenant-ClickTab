using iText.Commons.Utils;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClickTab.Core.DAL.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ClickTab.Core.DAL.Models.Generics;

namespace ClickTab.Core.HelperService.XLSUpdateHelper
{
    /// <summary>
    /// Servizio che si ccupa di aggiornare la tabella MENU tramite l'apposito file Xlsx
    /// L'attributo FileXlsImport serve ad attivare automaticamente la funzione di aggiornamento tramite FileXlsImportProviderFactory
    /// </summary>
    [FileXlsImport("Menu")]
    public class SyncMenuService : XlsUpdateBase<Menu>
    {
        public override DbSet<Menu> GetDbSet(DatabaseContext ctx)
=> ctx.Menu;

        public override void Sync(FileService fileService, ConfigurationService configService, DatabaseContext ctx, FileInfo info = null)
        {
            if (info == null)
            {
                string fileName = base.GetFileNameByAttribute();
                info = base.GetFileInfo(fileName);

            }

            if (!base.CheckUpdate(ctx, info))
                return;


            base.DefaultSync(ctx, info);

            base.UpdateEntity(ctx, info);

        }
    }
}

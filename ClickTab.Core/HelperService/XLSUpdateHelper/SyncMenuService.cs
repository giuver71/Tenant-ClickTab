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

namespace ClickTab.Core.HelperService.XLSUpdateHelper
{
    /// <summary>
    /// Servizio che si ccupa di aggiornare la tabella MENU tramite l'apposito file Xlsx
    /// L'attributo FileXlsImport serve ad attivare automaticamente la funzione di aggiornamento tramite FileXlsImportProviderFactory
    /// </summary>
    [FileXlsImport("Menu")]
    public class SyncMenuService : XlsUpdateBase
    {

        private const int MENU_ID_INDEX = 1;
        private const int MENU_LABEL_INDEX = 2;
        private const int MENU_CLIENTCODE_INDEX = 3;
        private const int MENU_FKPARENT_INDEX = 4;
        private const int MENU_ICON_INDEX = 5;
        private const int MENU_ORDER_INDEX = 6;
        //private const int MENU_ROLEDIMENSION_INDEX = 8; decremetato sottos
        private const int MENU_SYSTEMROLE_INDEX = 7;
        private const int MENU_URL_INDEX = 8;
        private const int MENU_ISEXTERNALPAGE_INDEX = 9;
        private const int MENU_NORMALLYHAVECHILDREN_INDEX = 10;
        private const int MENU_DELETED_INDEX = 11;

        public override void Sync(FileService _fileService,ConfigurationService _configService, DatabaseContext _ctx,FileInfo info)
        {

            if (info != null && !base.CheckUpdate(_ctx, info))
            {
                return;
            }

            string query = string.Empty;
            Console.WriteLine("Syncro MENU");
            try
            {
                Stream xlsxStream = _fileService.GetFileStreamFromResources("Resources.Menu.xlsx", AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "ClickTab.Web"));

                ExcelPackage xlsxPackage = new ExcelPackage(xlsxStream);
                var worksheet = xlsxPackage.Workbook.Worksheets[0];
                //Disabilito il controllo sui vincoli referenziali per evitare violazione di chiave
                _ctx.Database.ExecuteSqlRaw("Alter Table Menu NOCHECK CONSTRAINT all");

                for (int rowIndex = 2; rowIndex <= worksheet.Dimension.End.Row; rowIndex++)
                {
                    if (base.IsRowEmpty(worksheet, rowIndex))
                    {
                        continue;
                    }

                    if (worksheet.Cells[rowIndex, MENU_ID_INDEX].Value == null || worksheet.Cells[rowIndex, MENU_ID_INDEX].Value.ToString() == "")
                    {
                        throw new Exception($"Errore durante l'allineamento delle voci di menù: per la riga {rowIndex} del file Menu.xlsx non è stato indicato l'ID");
                    }

                    int fk_menu = worksheet.Cells[rowIndex, MENU_ID_INDEX].Value != null ? int.Parse(worksheet.Cells[rowIndex, MENU_ID_INDEX].Value.ToString()) : 0;

                    string label = worksheet.Cells[rowIndex, MENU_LABEL_INDEX].Value.ToString();
                    string ClientCode = worksheet.Cells[rowIndex, MENU_CLIENTCODE_INDEX].Value.ToString();
                  
                    string FK_Parent = worksheet.Cells[rowIndex, MENU_FKPARENT_INDEX].Value != null ? worksheet.Cells[rowIndex, MENU_FKPARENT_INDEX].Value.ToString() : "null";
                    string Icon = worksheet.Cells[rowIndex, MENU_ICON_INDEX].Value != null ? worksheet.Cells[rowIndex, MENU_ICON_INDEX].Value.ToString() : null;
                    int Order = worksheet.Cells[rowIndex, MENU_ORDER_INDEX].Value != null ? int.Parse(worksheet.Cells[rowIndex, MENU_ORDER_INDEX].Value.ToString()) : 0;
                    //string RoleDimension = worksheet.Cells[rowIndex, MENU_ROLEDIMENSION_INDEX].Value != null ? worksheet.Cells[rowIndex, MENU_ROLEDIMENSION_INDEX].Value.ToString() : "null";
                    string SystemRole = worksheet.Cells[rowIndex, MENU_SYSTEMROLE_INDEX].Value != null ? worksheet.Cells[rowIndex, MENU_SYSTEMROLE_INDEX].Value.ToString() : "null";
                    string Url = worksheet.Cells[rowIndex, MENU_URL_INDEX].Value.ToString();
                    int isExternalPage = worksheet.Cells[rowIndex, MENU_ISEXTERNALPAGE_INDEX].Value != null && worksheet.Cells[rowIndex, MENU_ISEXTERNALPAGE_INDEX].Value.ToString() == "1" ? 1 : 0;
                    int normallyHaveChildren = worksheet.Cells[rowIndex, MENU_NORMALLYHAVECHILDREN_INDEX].Value != null && worksheet.Cells[rowIndex, MENU_NORMALLYHAVECHILDREN_INDEX].Value.ToString() == "1" ? 1 : 0;

                    int deleted = worksheet.Cells[rowIndex, MENU_DELETED_INDEX].Value != null && worksheet.Cells[rowIndex, MENU_DELETED_INDEX].Value.ToString() == "1" ? 1 : 0;

                    query = @$"SET IDENTITY_INSERT [dbo].[Menu] ON 

                                   IF EXISTS (SELECT * FROM [dbo].[Menu] WHERE ID = {fk_menu}) 
                                    BEGIN 
                                        UPDATE Menu
                                        SET Label = '{label}',
                                            ClientCode = '{ClientCode}',
                                            FK_Parent = {FK_Parent},
                                            Icon = '{Icon}',
                                            [Order] = {Order},
                                            SystemRole = {SystemRole},
                                            Url = '{Url}',
                                            isExternalPage = {isExternalPage},
                                            normallyHaveChildren = {normallyHaveChildren},
                                            Deleted = {deleted},
                                            UpdateDate = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}' 
                                       WHERE ID={fk_menu} 
                                    END 
                                   ELSE 
                                    BEGIN
                                        INSERT INTO Menu (ID, Label, ClientCode,  FK_Parent, Icon, [Order], SystemRole,Url,isExternalPage,normallyHaveChildren, FK_InsertUser, FK_UpdateUser, InsertDate, UpdateDate,Deleted) 
                                        VALUES({fk_menu}, '{label}', '{ClientCode}',  {FK_Parent}, '{Icon}', {Order},  {SystemRole}, '{Url}', {isExternalPage}, {normallyHaveChildren}, -1, -1, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}',{deleted})
                                    END
                               SET IDENTITY_INSERT [dbo].[Menu] OFF";

                    _ctx.Database.ExecuteSqlRaw(query);
                }

                //Riabilito il controllo sui vincoli referenziali
                _ctx.Database.ExecuteSqlRaw("Alter Table Menu CHECK CONSTRAINT all");
                base.UpdateEntity(_ctx, info);
                System.IO.File.AppendAllText(base.pathService(), $"Sync MENU --> OK  {DateTime.Now} \n");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(base.pathService(), $"[ERR SyncDiaryParameterGroupParameters] {ex.Message}  {DateTime.Now} \n{query}");
                throw new Exception(ex.Message + "\r\n" + query);
            }

        }
       
    }
}

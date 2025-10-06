using ClickTab.Core.DAL.Context;
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
    public  class SyncRulesService :XlsUpdateBase
    {
        private const int RULES_ID_INDEX = 1;
        private const int RULES_URLROUTES_INDEX = 2;
        private const int RULES_DESCRIPTION_INDEX = 3;
        public override void Sync(FileService _fileService, ConfigurationService _configService, DatabaseContext _ctx, FileInfo info)
        {

            if (info != null && !base.CheckUpdate(_ctx, info))
            {
                return;
            }

            string query = string.Empty;
            Console.WriteLine("Syncro RULES");

            try
            {
                Stream xlsxStream = _fileService.GetFileStreamFromResources("Resources.Rules.xlsx", AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "ClickTab.Web"));

                ExcelPackage xlsxPackage = new ExcelPackage(xlsxStream);
                var worksheet = xlsxPackage.Workbook.Worksheets[0];
                //1 Disabilito il controllo sui vincoli referenziali per evitare violazione di chiave
                _ctx.Database.ExecuteSqlRaw("Alter Table Rules NOCHECK CONSTRAINT all");
                _ctx.Database.ExecuteSqlRaw("Alter Table RoleRules NOCHECK CONSTRAINT all");
                // 2 Cancello tutte le regole
                //bool applyDelete= Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToUpper()!="LOCAL";
               
                    // Si fa solo se diverso da Local
                    _ctx.Database.ExecuteSqlRaw("Delete from Rules");

                for (int rowIndex = 2; rowIndex <= worksheet.Dimension.End.Row; rowIndex++)
                {
                    if (base.IsRowEmpty(worksheet, rowIndex))
                    {
                        continue;
                    }

                    if (worksheet.Cells[rowIndex, RULES_ID_INDEX].Value == null || worksheet.Cells[rowIndex, RULES_ID_INDEX].Value.ToString() == "")
                    {
                        throw new Exception($"Errore durante l'allineamento delle Rules: per la riga {rowIndex} del file Rulkes.xlsx non è stato indicato l'ID");
                    }

                    int id = worksheet.Cells[rowIndex, RULES_ID_INDEX].Value != null ? int.Parse(worksheet.Cells[rowIndex, RULES_ID_INDEX].Value.ToString()) : 0;


                    string description = worksheet.Cells[rowIndex, RULES_DESCRIPTION_INDEX].Value != null ? worksheet.Cells[rowIndex, RULES_DESCRIPTION_INDEX].Value.ToString() : null;


                    string urlRoutes=   worksheet.Cells[rowIndex, RULES_URLROUTES_INDEX].Value != null ? worksheet.Cells[rowIndex, RULES_URLROUTES_INDEX].Value.ToString() : null;


                    // 3 Inserisco la regola prendendo i parametri dalle righe del foglio excel
                    query = @$"SET IDENTITY_INSERT [dbo].[Rules] ON 
                                    IF NOT EXISTS (SELECT * FROM [dbo].[Rules] WHERE ID = {id}) 
                                    BEGIN 
                                        INSERT INTO Rules (ID, Description, UrlRoutes) 
                                        VALUES({id}, '{description}', '{urlRoutes}')
                                    END
                                    ELSE
                                    BEGIN
                                        UPDATE Rules  SET 
                                            Description = '{description}',
                                            UrlRoutes='{urlRoutes}'
                                            WHERE ID={id}
                                    END
                               SET IDENTITY_INSERT [dbo].[Rules] OFF";

                    _ctx.Database.ExecuteSqlRaw(query);
                }




                    // 4 Cancello Tutte le roleRules "orfane" di Rule (cio avvienbe se dal foglio excel vengono rimosse delle regole)
                    // Solo in local 
                    _ctx.Database.ExecuteSqlRaw("Delete From RoleRules Where FK_Rule not in (Select ID from Rules)");



                // 5 Riabilito il controllo sui vincoli referenziali
                _ctx.Database.ExecuteSqlRaw("Alter Table Rules CHECK CONSTRAINT all");
                _ctx.Database.ExecuteSqlRaw("Alter Table RoleRules CHECK CONSTRAINT all");

                System.IO.File.AppendAllText(base.pathService(), $"Sync RULES --> OK  {DateTime.Now} \n");
                base.UpdateEntity(_ctx, info);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(base.pathService(), $"[ERR Sync RULES] {ex.Message}  {DateTime.Now} \n{query}");
                throw new Exception(ex.Message + "\r\n" + query);
            }
        }
    }
}

using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ClickTab.Core.HelperService.XLSUpdateHelper
{
    /// <summary>
    /// Servizio che si ccupa di aggiornare la tabella ROLES tramite l'apposito file Xlsx
    /// L'attributo FileXlsImport serve ad attivare automaticamente la funzione di aggiornamento tramite FileXlsImportProviderFactory
    /// </summary>
    [FileXlsImport("Roles")]
    public class SyncRolesService : XlsUpdateBase
    {
        public const int ROLES_ID_INDEX = 1;
        public const int ROLES_DESCRIPTION_INDEX = 2;


        public override void Sync(FileService _fileService,ConfigurationService _configService, DatabaseContext _ctx,FileInfo info)
        {
            // I ruoli vanno aggiornati asolo alla prima installazione
            if (_ctx.Roles.Count()>0)
            {
                return;
            }

            if (info != null && !base.CheckUpdate(_ctx, info))
            {
                return;
            }
            
            string query = string.Empty;
            

            Console.WriteLine("Syncro ROLES");
            try
            {
                Stream xlsxStream = _fileService.GetFileStreamFromResources("Resources.Roles.xlsx", AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "ClickTab.Web"));

                ExcelPackage xlsxPackage = new ExcelPackage(xlsxStream);
                var worksheet = xlsxPackage.Workbook.Worksheets[0];

                Console.WriteLine("Syncro ROLES");

                for (int rowIndex = 2; rowIndex <= worksheet.Dimension.End.Row; rowIndex++)
                {
                    if (base.IsRowEmpty(worksheet, rowIndex))
                    {
                        continue;
                    }

                    if (worksheet.Cells[rowIndex, ROLES_ID_INDEX].Value == null || worksheet.Cells[rowIndex, ROLES_ID_INDEX].Value.ToString() == "")
                    {
                        throw new Exception($"Errore durante l'allineamento dei Roles: per la riga {rowIndex} del file Roles.xlsx non è stato indicato l'ID");
                    }

                    int id = worksheet.Cells[rowIndex, ROLES_ID_INDEX].Value != null ? int.Parse(worksheet.Cells[rowIndex, ROLES_ID_INDEX].Value.ToString()) : 0;

                    string description = worksheet.Cells[rowIndex, ROLES_DESCRIPTION_INDEX].Value != null ? worksheet.Cells[rowIndex, ROLES_DESCRIPTION_INDEX].Value.ToString() : null;


                    query = @$"SET IDENTITY_INSERT [dbo].[Roles] ON 
                                    IF NOT EXISTS (SELECT * FROM [dbo].[Roles] WHERE ID = @id) 
                                    BEGIN 
                                        INSERT INTO Roles (ID, Description,                                         FK_InsertUser,InsertDate,FK_UpdateUser,UpdateDate) 
                                        VALUES(@ID, @Description, @FK_InsertUser,@InsertDate,@FK_UpdateUser,@UpdateDate)
                                    END
                               SET IDENTITY_INSERT [dbo].[Roles] OFF";
                    SqlConnection cnn = new SqlConnection(_ctx.Database.GetConnectionString());
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("Description", description);


                    cmd.Parameters.AddWithValue("FK_InsertUser", -1);

                    cmd.Parameters.AddWithValue("InsertDate", DateTime.Now);

                    cmd.Parameters.AddWithValue("FK_UpdateUser", -1);
                    cmd.Parameters.AddWithValue("UpdateDate", DateTime.Now);
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }
                base.UpdateEntity(_ctx, info);
                System.IO.File.AppendAllText(base.pathService(), $"SyncRoles --> OK  {DateTime.Now} \n");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(base.pathService(), $"[ERR SyncRoles] {ex.Message}  {DateTime.Now} \n{query}");
                throw new Exception(ex.Message + "\r\n" + query);
            }
        }
       
    }
}

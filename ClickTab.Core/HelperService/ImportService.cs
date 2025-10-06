using EQP.DocumentFiller.Core.Interfaces;
using EQP.EFRepository.Core.DAL;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ClickTab.Core.DAL.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.HelperService
{
    public class ImportService
    {
        private UnitOfWork<DatabaseContext> _uow;
        private DatabaseContext _ctx;
        private FileService _fileService;
        private ConfigurationService _configService;

        #region Indici file Menu.xlsx

        private const int MENU_ID_INDEX = 1;
        private const int MENU_LABEL_INDEX = 2;
        private const int MENU_CLIENTCODE_INDEX = 3;
        //private const int MENU_DELETED_INDEX = 4;
        private const int MENU_FKPARENT_INDEX = 4;
        private const int MENU_ICON_INDEX = 5;
        private const int MENU_ORDER_INDEX = 6;
        //private const int MENU_ROLEDIMENSION_INDEX = 8; decremetato sottos
        private const int MENU_SYSTEMROLE_INDEX = 7;
        private const int MENU_URL_INDEX = 8;
        private const int MENU_ISEXTERNALPAGE_INDEX = 9;
        private const int MENU_NORMALLYHAVECHILDREN_INDEX = 10;

        #endregion

        #region Indici file Placeholders.csv

        private const int PLACEHOLDER_ID_INDEX = 0;
        private const int PLACEHOLDER_NAME_INDEX = 1;
        private const int PLACEHOLDER_TAG_INDEX = 2;
        private const int PLACEHOLDER_TYPE_INDEX = 3;
        private const int PLACEHOLDER_SECTION_INDEX = 4;
        private const int PLACEHOLDER_FK_PARENT_INDEX = 5;
        private const int PLACEHOLDER_LIST_OF_ELEMENT_INDEX = 6;

        #endregion

        public ImportService(UnitOfWork<DatabaseContext> uow, DatabaseContext ctx, FileService fileService, ConfigurationService configService)
        {
            _uow = uow;
            _ctx = ctx;
            _fileService = fileService;
            _configService = configService;
        }

        /// <summary>
        /// Funzione che si occupa di tenere allineato il DB con il file Menu.xlsx memorizzato come risorsa incorporata del progetto web
        /// </summary>
        public void SyncMenu()
        {
            try
            {
                Stream xlsxStream = _fileService.GetFileStreamFromResources("Resources.Menu.xlsx", AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "ClickTab.Web"));

                ExcelPackage xlsxPackage = new ExcelPackage(xlsxStream);
                var worksheet = xlsxPackage.Workbook.Worksheets[0];

                //Disabilito il controllo sui vincoli referenziali per evitare violazione di chiave
                _ctx.Database.ExecuteSqlRaw("Alter Table Menu NOCHECK CONSTRAINT all");

                for (int rowIndex = 2; rowIndex <= worksheet.Dimension.End.Row; rowIndex++)
                {
                    if (IsRowEmpty(worksheet, rowIndex))
                        continue;

                    if (worksheet.Cells[rowIndex, MENU_ID_INDEX].Value == null || worksheet.Cells[rowIndex, MENU_ID_INDEX].Value.ToString() == "")
                        throw new Exception($"Errore durante l'allineamento delle voci di menù: per la riga {rowIndex} del file Menu.xlsx non è stato indicato l'ID");

                    int fk_menu = worksheet.Cells[rowIndex, MENU_ID_INDEX].Value != null ? int.Parse(worksheet.Cells[rowIndex, MENU_ID_INDEX].Value.ToString()) : 0;

                    string label = worksheet.Cells[rowIndex, MENU_LABEL_INDEX].Value.ToString();
                    string ClientCode = worksheet.Cells[rowIndex, MENU_CLIENTCODE_INDEX].Value.ToString();
                    //int Deleted = worksheet.Cells[rowIndex, MENU_DELETED_INDEX].Value != null && worksheet.Cells[rowIndex, MENU_DELETED_INDEX].Value.ToString() == "1" ? 1 : 0;
                    string FK_Parent = worksheet.Cells[rowIndex, MENU_FKPARENT_INDEX].Value != null ? worksheet.Cells[rowIndex, MENU_FKPARENT_INDEX].Value.ToString() : "null";
                    string Icon = worksheet.Cells[rowIndex, MENU_ICON_INDEX].Value != null ? worksheet.Cells[rowIndex, MENU_ICON_INDEX].Value.ToString() : null;
                    int Order = worksheet.Cells[rowIndex, MENU_ORDER_INDEX].Value != null ? int.Parse(worksheet.Cells[rowIndex, MENU_ORDER_INDEX].Value.ToString()) : 0;
                    //string RoleDimension = worksheet.Cells[rowIndex, MENU_ROLEDIMENSION_INDEX].Value != null ? worksheet.Cells[rowIndex, MENU_ROLEDIMENSION_INDEX].Value.ToString() : "null";
                    string SystemRole = worksheet.Cells[rowIndex, MENU_SYSTEMROLE_INDEX].Value != null ? worksheet.Cells[rowIndex, MENU_SYSTEMROLE_INDEX].Value.ToString() : "null";
                    string Url = worksheet.Cells[rowIndex, MENU_URL_INDEX].Value.ToString();
                    int isExternalPage = worksheet.Cells[rowIndex, MENU_ISEXTERNALPAGE_INDEX].Value != null && worksheet.Cells[rowIndex, MENU_ISEXTERNALPAGE_INDEX].Value.ToString() == "1" ? 1 : 0;
                    int normallyHaveChildren = worksheet.Cells[rowIndex, MENU_NORMALLYHAVECHILDREN_INDEX].Value != null && worksheet.Cells[rowIndex, MENU_NORMALLYHAVECHILDREN_INDEX].Value.ToString() == "1" ? 1 : 0;

                    string query = @$"SET IDENTITY_INSERT [dbo].[Menu] ON 

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
                                            UpdateDate = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}' 
                                       WHERE ID={fk_menu} 
                                    END 
                                   ELSE 
                                    BEGIN
                                        INSERT INTO Menu (ID, Label, ClientCode, FK_Parent, Icon, [Order], SystemRole,Url,isExternalPage,normallyHaveChildren, FK_InsertUser, FK_UpdateUser, InsertDate, UpdateDate) 
                                        VALUES({fk_menu}, '{label}', '{ClientCode}', {FK_Parent}, '{Icon}', {Order},  {SystemRole}, '{Url}', {isExternalPage}, {normallyHaveChildren}, -1, -1, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}')
                                    END
                               SET IDENTITY_INSERT [dbo].[Menu] OFF";

                    _ctx.Database.ExecuteSqlRaw(query);
                }

                //Riabilito il controllo sui vincoli referenziali
                _ctx.Database.ExecuteSqlRaw("Alter Table Menu CHECK CONSTRAINT all");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sincronizza i placeholders per i modelli dell'editor
        /// </summary>
        //public void SyncPlaceholders()
        //{
        //    try
        //    {

        //        Stream csvStream = _fileService.GetFileStreamFromResources("Resources.Placeholders.csv", AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "ClickTab.Web"));

        //        //Elimina tutti i placeholder per poi ricrearli nuovi
        //        //In questo modo non è necessario accedere più volte al DB per controllare l'esistenza di un placeholder per sapere se crearlo o modificarlo
        //        //Questa operazione è consentita solo perchè i placeholder non hanno relazioni con nessuna entità quindi non si rischia di violare nessuna referenza
        //        using (DatabaseContext ctx = new DatabaseContext(_configService.ConnectionString, _configService.DbContextProvider))
        //        {
        //            IQueryable<Placeholder> allPlaceholdersToRemove = ctx.Placeholders;
        //            ctx.Placeholders.RemoveRange(allPlaceholdersToRemove);
        //            ctx.SaveChanges();

        //            using (var reader = new StreamReader(csvStream))
        //            {
        //                while (!reader.EndOfStream)
        //                {
        //                    string csvLine = reader.ReadLine();
        //                    string[] splittedLine = csvLine.Split(';');

        //                    Placeholder placeholder = new Placeholder()
        //                    {
        //                        ID = int.Parse(splittedLine[PLACEHOLDER_ID_INDEX]),
        //                        Tag = splittedLine[PLACEHOLDER_TAG_INDEX],
        //                        PlaceholderType = (EQP.DocumentFiller.Core.Interfaces.PlaceholderType)int.Parse(splittedLine[PLACEHOLDER_TYPE_INDEX]),
        //                        TemplateSection = !string.IsNullOrEmpty(splittedLine[PLACEHOLDER_SECTION_INDEX]) && splittedLine[PLACEHOLDER_SECTION_INDEX] != "NULL" ? (TemplateSection)int.Parse(splittedLine[PLACEHOLDER_SECTION_INDEX]) : (TemplateSection?)null,
        //                        FK_Parent = !string.IsNullOrEmpty(splittedLine[PLACEHOLDER_FK_PARENT_INDEX]) && splittedLine[PLACEHOLDER_FK_PARENT_INDEX] != "NULL" ? int.Parse(splittedLine[PLACEHOLDER_FK_PARENT_INDEX]) : (int?)null,
        //                        IsListOfElements = !string.IsNullOrEmpty(splittedLine[PLACEHOLDER_LIST_OF_ELEMENT_INDEX]) && (splittedLine[PLACEHOLDER_LIST_OF_ELEMENT_INDEX] == "1" || splittedLine[PLACEHOLDER_LIST_OF_ELEMENT_INDEX].ToLower() == "true") ? true : false,
        //                        Name = splittedLine[PLACEHOLDER_NAME_INDEX]
        //                    };
        //                    ctx.Placeholders.Add(placeholder);
        //                }

        //                ctx.Database.OpenConnection();
        //                ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Placeholders ON");
        //                ctx.SaveChanges();
        //                ctx.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Placeholders OFF");
        //                ctx.Database.CloseConnection();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #region Funzioni private

        /// <summary>
        /// Verifica se una specifica riga del worksheet risulta vuota oppure no
        /// </summary>
        /// <param name="sheet">Worksheet da controllare</param>
        /// <param name="row">Riga da controllare</param>
        /// <returns>Restituisce TRUE se l'intera riga risulta vuota, altrimenti restituisce FALSE</returns>
        private bool IsRowEmpty(ExcelWorksheet sheet, int row)
        {
            bool isEmpty = true;

            for (int colIndex = 1; colIndex <= sheet.Dimension.End.Column; colIndex++)
            {
                if (sheet.Cells[row, colIndex].Value != null)
                    isEmpty = false;
            }

            return isEmpty;
        }

        #endregion
    }
}

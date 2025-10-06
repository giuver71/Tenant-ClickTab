using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Models.Generics;

namespace ClickTab.Core.HelperService.XLSUpdateHelper
{
    public interface IXlsUpdate
    {
        /// <summary>
        /// Effettua la sincronizzazione fra il foglio Excel e la tabella relativa
        /// </summary>
        /// <param name="fileService"></param>
        /// <param name="configService"></param>
        /// <param name="_ctx"></param>
        /// <param name="info"></param>
        public void Sync(FileService fileService, ConfigurationService configService, DatabaseContext _ctx, FileInfo info = null);

        /// <summary>
        /// Verifica la necessità di aggionare confrontando la data di ultima modifica del file XLS rispetto alla data di Ultimo aggiornamento 
        /// </summary>
        /// <param name="_ctx"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool CheckUpdate(DatabaseContext _ctx, FileInfo info);

        /// <summary>
        /// Aggiorna la tabella UpdateXLS con i dati del file appena elaborato
        /// </summary>
        /// <param name="_ctx"></param>
        /// <param name="info"></param>
        public void UpdateEntity(DatabaseContext _ctx, FileInfo info);


        public string GetFileNameByAttribute();

    }

   
    public abstract class XlsUpdateBase : IXlsUpdate
    {
        public static readonly string PROJECT_FOLDER_BASE_PATH = "ClickTabFILES";
        public abstract void Sync(FileService fileService, ConfigurationService configService, DatabaseContext _ctx, FileInfo info=null);

        /// <summary>
        /// Aggiorna ogni singola dipendenza 
        /// </summary>
        /// <param name="fileService"></param>
        /// <param name="configService"></param>
        /// <param name="ctx"></param>
        /// <exception cref="Exception"></exception>
        protected void SyncDependencies(FileService fileService, ConfigurationService configService, DatabaseContext ctx)
        {
            var attr = this.GetType().GetCustomAttribute<FileXlsImportAttribute>();
            if (attr?.Dependencies != null)
            {
                foreach (var dep in attr.Dependencies)
                {
                    string path = $@"Resources\{dep}.xlsx";
                    if (!File.Exists(path))
                    {
                        continue;
                    }
                    FileInfo fileInfo = new FileInfo(path);
                    IXlsUpdate depService = FileXlsImportProviderFactory.ActivatorServiceSyncroToXls(dep);
                    if (depService==null)
                    {
                        throw new Exception($"Il servizio di Aggiornamento per il file di dipendenze {dep} non è disponibile nel catalogo!");
                    }
                    depService.Sync(fileService, configService, ctx, fileInfo);
                }
            }
        }
        
        /// <summary>
        /// testa se la tabella è realmente da aggiornare
        /// <remarks>
        /// Il test viene eseguito confrontando l'has del file con quello della tabella
        /// </remarks>
        /// </summary>
        /// <param name="_ctx"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool CheckUpdate(DatabaseContext _ctx, FileInfo info)
        {
            var updateXls = _ctx.UpdateXls.AsNoTracking().FirstOrDefault(p => p.FileName == GetFileNameByAttribute());
            string hash = GenerateContentHash(info);
            return updateXls == null || updateXls.Hash != hash;
        }


        /// <summary>
        /// Aggiorna la tabella UpdateXls per i successivi confronti
        /// </summary>
        /// <param name="_ctx"></param>
        public void UpdateEntity(DatabaseContext _ctx,FileInfo info)
        {

           

            var updateXls = _ctx.UpdateXls.AsNoTracking().FirstOrDefault(p => p.FileName == GetFileNameByAttribute());

            //var existing = _ctx.ChangeTracker.Entries<UpdateXls>()
            //         .FirstOrDefault(e => e.Entity.ID == updateXls.ID);
            //if (existing != null)
            //    existing.State = EntityState.Detached;

            //_ctx.Attach(updateXls);


            if (updateXls != null)
            {
                updateXls.LastUpdate = DateTime.Now;
                updateXls.Hash = GenerateContentHash(info);
                _ctx.Update(updateXls);
            }
            else
            {
                updateXls = new UpdateXls
                {
                    LastUpdate = DateTime.Now,
                    FileName = GetFileNameByAttribute(),
                    Hash = GenerateContentHash(info),
                    UpdateDate = DateTime.Now,
                    InsertDate = DateTime.Now
                };
                _ctx.Add(updateXls);
            }

            _ctx.SaveChanges();
        }


        /// <summary>
        /// Calcola l'hash sul contenuto logico di ogni file Excel
        /// </summary>
        /// <remarks>
        /// il file .xlsx è in realtà un archivio ZIP contenente più file XML (e altri eventuali file di supporto).Anche senza cambiare i dati visibili nel foglio di lavoro, Excel può aggiornare metadati o informazioni temporali ogni volta che salvi.Questo provoca una variazione del contenuto interno del file, e quindi il digest hash (come SHA-256 o MD5) cambia.
        /// </remarks>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GenerateContentHash(FileInfo file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var rowsData = new List<string>();

            using var package = new ExcelPackage(file);
            var workbook = package.Workbook;

            foreach (var sheet in workbook.Worksheets)
            {
                var dimension = sheet.Dimension;
                if (dimension == null)
                    continue; // foglio vuoto

                for (int row = dimension.Start.Row; row <= dimension.End.Row; row++)
                {
                    var rowValues = new List<string>();

                    for (int col = dimension.Start.Column; col <= dimension.End.Column; col++)
                    {
                        var cell = sheet.Cells[row, col];
                        if (cell?.Value == null)
                        {
                            rowValues.Add(""); // normalizzo: cella vuota = stringa vuota
                        }
                        else
                        {
                            // Normalizzo spazi e maiuscole/minuscole (se vuoi case-insensitive)
                            var value = cell.Value.ToString().Trim();
                            rowValues.Add(value);
                        }
                    }

                    // Se la riga è completamente vuota la salto
                    if (rowValues.All(v => string.IsNullOrEmpty(v)))
                        continue;

                    rowsData.Add(string.Join("|", rowValues));
                }
            }

            // Ordino le righe se l’ordine non conta
            // rowsData.Sort();

            var normalizedText = string.Join("\n", rowsData);

            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(normalizedText);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "");
        }

      
        /// <summary>
        /// Genera un Hash del file
        /// </summary>
        /// <remarks>
        /// Non utilizzata nel caso di file XLSX
        /// </remarks>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GenerateHash(FileInfo info)
        {
            using (var stream = File.OpenRead(info.FullName))
            {
                using var sha = SHA256.Create();
                byte[] hash = sha.ComputeHash(stream);
                string hex = BitConverter.ToString(hash).Replace("-", "");
                return hex;
            }
        }

        /// <summary>
        /// Prende l'Id della struttura di default
        /// </summary>
        /// <param name="_ctx"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public int  GetDefaultIDFacilitie(DatabaseContext _ctx)
        {
            //Facilitie facilitie = _ctx.Facilities.Where(x => x.IsDefault ).FirstOrDefault();
            //if (facilitie == null)
            //{
            //    facilitie = (_ctx.Facilities.Count() == 1) ? _ctx.Facilities.FirstOrDefault() : null;
            //    if (facilitie == null)
            //    {
            //        throw new System.Exception("Missing default facilitiy. Please contact the administrator");
            //    }
            //}
            return 0;
        }

        /// <summary>
        /// Tramite l'attributo FileXlsImportAttribute restituisce il noem del file 
        /// </summary>
        /// <returns></returns>
        public string GetFileNameByAttribute()
        {
            var attr = this.GetType().GetCustomAttribute<FileXlsImportAttribute>();
            return attr?.Filename;
        }


        /// <summary>
        /// Per la creazione utente cr
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public String EncryptSHA256(String value)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));
                foreach (Byte b in result)
                {
                    Sb.Append(b.ToString("x2"));
                }
            }
            return Sb.ToString();
        }

        public string pathService()
        {
            return Directory.GetCurrentDirectory() + "/logs/SyncroLogs.txt";
        }

        public bool IsRowEmpty(ExcelWorksheet sheet, int row)
        {
            bool isEmpty = true;

            for (int colIndex = 1; colIndex <= sheet.Dimension.End.Column; colIndex++)
            {
                if (sheet.Cells[row, colIndex].Value != null)
                {
                    isEmpty = false;
                }
            }

            return isEmpty;
        }


        public bool UploadFile(byte[] FileData, string FilePath,ConfigurationService _configService)
        {
            bool result = false;
            
            switch (_configService.FileStorageMode)
            {
                case FileStorageMode.ProjectFolder:
                    string fileFullPath = Path.Combine(Environment.CurrentDirectory, PROJECT_FOLDER_BASE_PATH, FilePath);

                    //Crea la directory, se necessario
                    string fileDirectory = Path.GetDirectoryName(fileFullPath);
                    if (!Directory.Exists(fileDirectory))
                        Directory.CreateDirectory(fileDirectory);

                    File.WriteAllBytes(fileFullPath, FileData);
                    result = true;
                    break;
            }


            return result;
        }
    }
}

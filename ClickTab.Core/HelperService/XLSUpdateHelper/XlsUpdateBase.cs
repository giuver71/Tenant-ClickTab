using ClickTab.Core.DAL.Context;
using ClickTab.Core.DAL.Enums;
using ClickTab.Core.DAL.Models.Generics;
using EQP.EFRepository.Core.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        /// Verifica la necessità di aggionare confrontando l'HASH del foglio corrente con l'HASH memorizzato nel DB relatico all'ultimo aggiornamento
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


        /// <summary>
        /// Recupera il nome del File XLSX tramite l'attributo FileXlsImport di ogni singolo servizio
        /// </summary>
        /// <returns></returns>
        public string GetFileNameByAttribute();

    }

    public abstract class XlsUpdateBase<T> : IXlsUpdate where T : class, new()
    {
        private bool hasChangedRecords = false;
        protected List<Dictionary<string, object>> UpdateXlsAction { get; private set; }

        /// <summary>
        /// Carica e memorizza il contenuto del file Excel di Controllo delle Azioni, visibile a tutta la classe
        /// </summary>
        public void LoadUpdateXlsAction()
        {
            string folderPath = @"Resources";
            FileInfo info = GetFileInfo("UpdateXlsAction");
            if (info == null)
            {
                throw new Exception("File di controllo UpdateXlsAction non trovato");
            }
            UpdateXlsAction = ReadExcel(info);
        }


        public abstract DbSet<T> GetDbSet(DatabaseContext ctx);



        public abstract void Sync(FileService fileService, ConfigurationService configService, DatabaseContext ctx, FileInfo info = null);

        /// <summary>
        /// Aggiorna le dipendenze in funzione della proprieta dell'Attributo Dependencies
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
                    if (depService == null)
                    {
                        throw new Exception($"Il servizio di Aggiornamento per il file di dipendenze {dep} non è disponibile nel catalogo!");
                    }
                    depService.Sync(fileService, configService, ctx, fileInfo);
                }
            }
        }


        public virtual List<Dictionary<string, object>> ReadExcel(FileInfo file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var rows = new List<Dictionary<string, object>>();

            using var package = new ExcelPackage(file);
            var ws = package.Workbook.Worksheets.First();
            var colCount = ws.Dimension.End.Column;
            var rowCount = ws.Dimension.End.Row;

            // header
            var headers = new List<string>();
            for (int col = 1; col <= colCount; col++)
                headers.Add(ws.Cells[1, col].Text.Trim());

            // rows
            for (int row = 2; row <= rowCount; row++)
            {
                var dict = new Dictionary<string, object>();
                for (int col = 1; col <= colCount; col++)
                    dict[headers[col - 1]] = ws.Cells[row, col].Value;
                rows.Add(dict);
            }

            return rows;
        }






        #region LOGICA DI DEFAULT DI SYNC (opzionale, si può fare override)

        /// <summary>
        /// Metodo di Default che effettua la sincronizzazione
        /// </summary>
        /// <remarks>
        /// Nel dizionario additionalColumns vengono aggiunte quelle colonne che non sono nel Foglio Excel ma che vanno ugualmente trattate nelle Insert e nelle Update --> Esempio La FK_Facilitie che viene recuperata tramite la GetDefaultIDFacilitie
        /// per vedere un esempio andare SyncPerformanceTypesService
        /// </remarks>
        /// <param name="ctx"></param>
        /// <param name="info"></param>
        /// <param name="skipUpdateMethod">
        /// Mettendo a true questo parametro la sincronizzazione lavorerà solo in INSERIMENTO
        /// questo per quelle tabelle che non possono essere soggette a modifica tramitre FOGLI EXCEL dopo il primo inserimento
        /// come ad esempio la FACILITIE (struttura)
        /// </param>
        /// <param name="useSql"></param>
        /// <param name="useAudit"></param>
        /// <param name="useSoftDelete"></param>
        protected void DefaultSync(DatabaseContext ctx, FileInfo info, Dictionary<string, object> additionalColumns = null)
        {
            hasChangedRecords = false;
            if (UpdateXlsAction == null)
                LoadUpdateXlsAction();

            string fileAction = GetFileNameByAttribute();

            var rowAction = UpdateXlsAction.FirstOrDefault(r => r.TryGetValue("File", out var value) &&
                string.Equals(value?.ToString(), fileAction, StringComparison.OrdinalIgnoreCase));
            if (rowAction == null)
            {
                throw new Exception($"Nel file UpdateXlsAction non hai definitole azioni per {fileAction}");
            }

            bool useSqlServer = false;
            if (rowAction.TryGetValue("UseSqlServer", out var rawUseSql))
                useSqlServer = rawUseSql is double d ? d == 1 : rawUseSql?.ToString() == "1";

            DbActionEnum dbAction = DbActionEnum.ONLY_FIRST_START;
            if (rowAction.TryGetValue("DBActionEnum", out var rawAction))
            {
                Enum.TryParse<DbActionEnum>(rawAction.ToString(), out DbActionEnum parsedEnum);
                dbAction = parsedEnum;

            }



            if (dbAction == DbActionEnum.ONLY_FIRST_START)
            {
                bool hasRecords = GetDbSet(ctx).AsNoTracking().Any();
                if (hasRecords)
                {
                    return;
                }

            }



            var excelRows = ReadExcel(info);
            var dbSet = GetDbSet(ctx);
            var list = dbSet.AsNoTracking().ToList();
            string tableName = GetTableName(ctx);
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine($"Start Sync {info.Name.ToUpper()} - {DateTime.Now}");

            using (var transaction = ctx.Database.BeginTransaction())
            {
                try
                {
                    if (typeof(IBaseEntity).IsAssignableFrom(typeof(T)))
                        ctx.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[{tableName}] ON");

                    int index = 1;
                    foreach (var row in excelRows)
                    {
                        double percent = ((double)index / excelRows.Count) * 100;
                        Console.Write($"\rSto Elaborando riga {index} di {excelRows.Count} ({percent:0}%)");

                        int id = Convert.ToInt32(row["ID"]);
                        // Cerco nella lista del DbSet il record con I'ID
                        var current = list.FirstOrDefault(e =>
                            (int)typeof(T).GetProperty("ID").GetValue(e) == id);
                        // Se non viene trovato Inserisco
                        if (current == null)
                        {
                            // Quando si USA SQlServer per l'inseriemnto o l'Update l'aggiornamento avviene rigfa x riga
                            // dal momento che si usa un SQlCommand
                            // nel caso Di EF CORE l'aggiornamento è globale ed avviente al savechanges del metodo CommitIfNeeded
                            if (useSqlServer && (dbAction == DbActionEnum.ONLY_FIRST_START || dbAction == DbActionEnum.ONLY_INSERT || dbAction == DbActionEnum.INSERT_AND_UPDATE))
                            {
                                InsertRow(ctx, tableName, row, additionalColumns);
                            }
                            else if (!useSqlServer && (dbAction == DbActionEnum.ONLY_FIRST_START || dbAction == DbActionEnum.ONLY_INSERT || dbAction == DbActionEnum.INSERT_AND_UPDATE))
                            {

                                InsertRowByDbSet(row, dbSet, ctx, additionalColumns);
                            }
                        }
                        else
                        {
                            if (dbAction == DbActionEnum.INSERT_AND_UPDATE && useSqlServer && IsDifferent(row, current))
                            {
                                UpdateRow(ctx, tableName, row);
                            }
                            else if (dbAction == DbActionEnum.INSERT_AND_UPDATE && !useSqlServer && IsDifferent(row, current))
                            {
                                UpdateRowByDbSet(current, row, dbSet, additionalColumns);
                            }
                        }




                        index++;
                    }

                    CommitIfNeeded(ctx, useSqlServer);

                    if (typeof(IBaseEntity).IsAssignableFrom(typeof(T)))
                        ctx.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[{tableName}] OFF");

                    transaction.Commit();

                    File.AppendAllText(pathService(), $"Sync {info.Name} --> OK  {DateTime.Now} \n");
                    Console.WriteLine($"\nEnd Sync {info.Name.ToUpper()} - {DateTime.Now}");
                    Console.WriteLine("------------------------------------------------------------------------");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (typeof(T) is IBaseEntity)
                        ctx.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[{tableName}] OFF");

                    File.AppendAllText(pathService(), $"[ERR Sync {info.Name}] {ex.Message}  {DateTime.Now}\n");
                    throw;
                }
            }
        }


        protected void CommitIfNeeded(DatabaseContext ctx, bool useSql)
        {
            if (!useSql && hasChangedRecords)
            {
                ctx.SaveChanges();
            }
            // Se useSql == true, non serve perché ExecuteSqlRaw ha già fatto commit
        }

        //protected List<Dictionary<string, object>> ReadExcel(FileInfo file)
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    var rows = new List<Dictionary<string, object>>();

        //    using var package = new ExcelPackage(file);
        //    var ws = package.Workbook.Worksheets.First();
        //    var colCount = ws.Dimension.End.Column;
        //    var rowCount = ws.Dimension.End.Row;

        //    // header
        //    var headers = new List<string>();
        //    for (int col = 1; col <= colCount; col++)
        //        headers.Add(ws.Cells[1, col].Text.Trim());

        //    // rows
        //    for (int row = 2; row <= rowCount; row++)
        //    {
        //        var dict = new Dictionary<string, object>();
        //        for (int col = 1; col <= colCount; col++)
        //            dict[headers[col - 1]] = ws.Cells[row, col].Value;
        //        rows.Add(dict);
        //    }

        //    return rows;
        //}




        // ========================================================
        // == COMPARATORE BASE PER DIFFERENZE
        // ========================================================

        /// <summary>
        /// Verfica eventuali differenze fra la rioga corrente del foglio e il dato effettivo nel DB recuperato e dato come input in entity
        /// </summary>
        /// <param name="row"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual bool IsDifferent(Dictionary<string, object> row, T entity)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.Name == "ID") continue;
                if (!row.ContainsKey(prop.Name)) continue;

                var dbValue = prop.GetValue(entity);
                var excelValue = row[prop.Name];

                if (dbValue == null && excelValue == null)
                    continue;
                if (dbValue == null || excelValue == null)
                    return true;

                var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                try
                {
                    object convertedExcelValue = excelValue;

                    // 🔹 Caso Enum
                    if (targetType.IsEnum)
                    {
                        if (excelValue is string s)
                        {
                            // accetta sia nome enum sia numero
                            if (int.TryParse(s, out int numeric))
                                convertedExcelValue = Enum.ToObject(targetType, numeric);
                            else
                                convertedExcelValue = Enum.Parse(targetType, s, ignoreCase: true);
                        }
                        else if (excelValue is double d)
                        {
                            convertedExcelValue = Enum.ToObject(targetType, (int)d);
                        }
                        else
                        {
                            convertedExcelValue = Enum.ToObject(targetType, excelValue);
                        }
                    }
                    else
                    {
                        convertedExcelValue = Convert.ChangeType(excelValue, targetType);
                    }

                    // 🔹 Confronto logico
                    if (!object.Equals(dbValue, convertedExcelValue))
                        return true;
                }
                catch
                {
                    // fallback: confronto testuale
                    if (!string.Equals(dbValue?.ToString()?.Trim(), excelValue?.ToString()?.Trim(), StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }


        #endregion

        #region VERSIONE GENERICA CON EF CORE (DbSet)

        protected void InsertRowByDbSet<T>(Dictionary<string, object> row, DbSet<T> dbSet, DatabaseContext ctx, Dictionary<string, object> additionalColumns
) where T : class, new()
        {
            var entity = new T();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (!row.ContainsKey(prop.Name))
                    continue;

                var rawValue = row[prop.Name];
                if (rawValue == null || string.IsNullOrWhiteSpace(rawValue.ToString()))
                    continue;

                object safeValue;
                var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                try
                {
                    if (targetType.IsEnum)
                    {
                        if (rawValue is string s)
                        {
                            if (int.TryParse(s, out int numericValue))
                                safeValue = Enum.ToObject(targetType, numericValue);
                            else
                                safeValue = Enum.Parse(targetType, s, true);
                        }
                        else
                        {
                            var underlyingType = Enum.GetUnderlyingType(targetType);
                            var numericValue = Convert.ChangeType(rawValue, underlyingType);
                            safeValue = Enum.ToObject(targetType, numericValue);
                        }
                    }
                    else
                    {
                        safeValue = Convert.ChangeType(rawValue, targetType);
                    }

                    prop.SetValue(entity, safeValue);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Errore conversione proprietà {prop.Name}: {ex.Message}");
                }
            }

            // Applica le colonne aggiuntive tramite la funzione esterna
            ApplyAdditionalColumns(entity, additionalColumns);

            dbSet.Add(entity);
            hasChangedRecords = true;
        }


        protected void UpdateRowByDbSet<T>(T entityToUpdate, Dictionary<string, object> row, DbSet<T> dbSet, Dictionary<string, object> additionalColumns) where T : class, new()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.Name == "ID")
                    continue; // non toccare mai la PK

                if (!row.ContainsKey(prop.Name))
                    continue;

                var rawValue = row[prop.Name];
                if (rawValue == null || string.IsNullOrWhiteSpace(rawValue.ToString()))
                    continue;

                try
                {
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    object safeValue;

                    // 🔹 Caso Enum
                    if (targetType.IsEnum)
                    {
                        if (rawValue is string s)
                        {
                            if (int.TryParse(s, out int numericValue))
                                safeValue = Enum.ToObject(targetType, numericValue);
                            else
                                safeValue = Enum.Parse(targetType, s, true);
                        }
                        else
                        {
                            var underlyingType = Enum.GetUnderlyingType(targetType);
                            var numericValue = Convert.ChangeType(rawValue, underlyingType);
                            safeValue = Enum.ToObject(targetType, numericValue);
                        }
                    }
                    else
                    {
                        safeValue = Convert.ChangeType(rawValue, targetType);
                    }

                    prop.SetValue(entityToUpdate, safeValue);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Errore conversione proprietà {prop.Name}: {ex.Message}");
                }
            }
            // Applico le colonne Addizionali
            ApplyAdditionalColumns(entityToUpdate, additionalColumns);
            dbSet.Update(entityToUpdate);
            hasChangedRecords = true;

        }

        protected void ApplyAdditionalColumns<T>(T entity, Dictionary<string, object> additionalColumns)
        {
            if (entity == null || additionalColumns == null)
                return;

            foreach (var kvp in additionalColumns)
            {
                var prop = typeof(T).GetProperty(kvp.Key);
                if (prop == null || !prop.CanWrite)
                    continue;

                var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                try
                {
                    object safeValue = kvp.Value == null
                        ? null
                        : Convert.ChangeType(kvp.Value, targetType);

                    prop.SetValue(entity, safeValue);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Errore conversione proprietà aggiuntiva {prop.Name}: {ex.Message}");
                }
            }
        }


        #endregion


        #region VERSIONE GENERICA CON SQL RAW

        protected void InsertRow(DatabaseContext ctx, string tableName, Dictionary<string, object> row, Dictionary<string, object> additionalColumns = null)
        {
            if (additionalColumns != null)
            {
                foreach (var kvp in additionalColumns)
                {
                    if (!row.ContainsKey(kvp.Key))
                    {
                        row[kvp.Key] = kvp.Value;
                    }
                }
            }
            var columns = row.Keys.ToList();
            //if (!typeof(IBaseEntity).IsAssignableFrom(typeof(T)))
            //{
            //    columns.Remove("ID");
            //}


            //Aggiungo sempre i campi di auditing se non presenti
            if (typeof(IAuditEntity<int>).IsAssignableFrom(typeof(T)))
            {

                if (!columns.Contains("FK_InsertUser"))
                {
                    columns.Add("FK_InsertUser");
                    row["FK_InsertUser"] = -1;
                }
                if (!columns.Contains("InsertDate"))
                {
                    columns.Add("InsertDate");
                    row["InsertDate"] = DateTime.Now;
                }

                if (!columns.Contains("FK_UpdateUser"))
                {
                    columns.Add("FK_UpdateUser");
                    row["FK_UpdateUser"] = -1;
                }
                if (!columns.Contains("UpdateDate"))
                {
                    columns.Add("UpdateDate");
                    row["UpdateDate"] = DateTime.Now;
                }
            }

            //Aggiungi sempre i campi di SoftDelete se non presenti
            if (typeof(ISoftDeleteEntity).IsAssignableFrom(typeof(T)))
            {

                if (!columns.Contains("Deleted"))
                {
                    columns.Add("Deleted");
                    row["Deleted"] = 0;
                }
                if (!columns.Contains("FK_DeletedUser"))
                {
                    columns.Add("FK_DeletedUser");
                    row["FK_DeletedUser"] = -1;
                }
                if (!columns.Contains("DeletedDate"))
                {
                    columns.Add("DeletedDate");
                    row["DeletedDate"] = DBNull.Value;
                }
            }
            var colList = string.Join(",", columns.Select(c => $"[{c}]"));
            var paramList = string.Join(",", columns.Select(c => "@" + c));

            string sql = "";
            sql += $"INSERT INTO {tableName} ({colList}) VALUES ({paramList})";

            using var cmd = ctx.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;

            //Collega la transazione EF se esiste
            var transaction = ctx.Database.CurrentTransaction;
            if (transaction != null)
                cmd.Transaction = transaction.GetDbTransaction();

            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                cmd.Connection.Open();
            string stringParameters = string.Empty;
            foreach (var col in columns)
            {
                var p = cmd.CreateParameter();
                p.ParameterName = "@" + col;
                p.Value = row[col] ?? DBNull.Value;
                cmd.Parameters.Add(p);
                stringParameters += $"{p.ParameterName} || {p.Value.ToString()}";
            }
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message} - Collection Parametri {stringParameters}");
            }
        }



        protected void UpdateRow(DatabaseContext ctx, string tableName, Dictionary<string, object> row, Dictionary<string, object> additionalColumns = null, string keyColumn = "ID", bool useAudit = true, bool useSoftDelete = true)
        {
            if (additionalColumns != null)
            {
                foreach (var kvp in additionalColumns)
                {
                    if (!row.ContainsKey(kvp.Key))
                    {
                        row[kvp.Key] = kvp.Value;
                    }
                }
            }
            var columns = row.Keys.ToList();

            //Aggiungi sempre i campi di auditing se non presenti
            if (typeof(IAuditEntity<int>).IsAssignableFrom(typeof(T)))
            {

                if (!columns.Contains("FK_UpdateUser"))
                {
                    columns.Add("FK_UpdateUser");
                    row["FK_UpdateUser"] = -1;
                }
                if (!columns.Contains("UpdateDate"))
                {
                    columns.Add("UpdateDate");
                    row["UpdateDate"] = DateTime.Now;
                }
            }
            if (typeof(ISoftDeleteEntity).IsAssignableFrom(typeof(T)))
            {

                if (!columns.Contains("Deleted"))
                {
                    columns.Add("Deleted");
                    row["Deleted"] = 1;
                }
                if (!columns.Contains("FK_DeletedUser"))
                {
                    columns.Add("FK_DeletedUser");
                    row["FK_DeletedUser"] = -1;
                }
                if (!columns.Contains("DeletedDate"))
                {
                    columns.Add("DeletedDate");
                    row["DeletedDate"] = DateTime.Now;
                }
            }


            columns.Remove(keyColumn);
            var setList = string.Join(",", columns.Select(c => $"[{c}]=@{c}"));

            string sql = $"UPDATE {tableName} SET {setList} WHERE {keyColumn}=@{keyColumn}";

            using var cmd = ctx.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;

            //Collega la transazione EF se esiste
            var transaction = ctx.Database.CurrentTransaction;
            if (transaction != null)
                cmd.Transaction = transaction.GetDbTransaction();

            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                cmd.Connection.Open();


            foreach (var col in row.Keys)
            {
                var p = cmd.CreateParameter();
                p.ParameterName = "@" + col;
                p.Value = row[col] ?? DBNull.Value;
                cmd.Parameters.Add(p);
            }



            cmd.ExecuteNonQuery();
        }


        #endregion


        /// <summary>
        /// Recupera i nome della tabella nal Database corrente 
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected string GetTableName(DatabaseContext ctx)
        {
            var entity = ctx.Model.FindEntityType(typeof(T));
            if (entity == null)
                throw new Exception($"Entity type {typeof(T).Name} not found in DbContext.");

            var tableName = entity.GetTableName();
            var schema = entity.GetSchema();

            return string.IsNullOrEmpty(schema) ? tableName : $"{schema}.{tableName}";
        }





        //private DbActionEnum ConvertToDbActionEnum(object rawValue)
        //{
        //    if (rawValue == null)
        //        return DbActionEnum.ONLY_INSERT; // default di sicurezza

        //    string value = rawValue.ToString().Trim();

        //    // Proviamo il parsing case-insensitive
        //    if (Enum.TryParse<DbActionEnum>(value, ignoreCase: true, out var result))
        //        return result;

        //    // Se non riconosciuto, log e default
        //    return DbActionEnum.ONLY_FIRST_START;


        public bool ConvertToBool(object value)
        {
            if (value == null)
                return false;

            if (value is double d)
                return d == 1;

            if (value is int i)
                return i == 1;

            if (value is string s && double.TryParse(s, out var num))
                return num == 1;

            return false;
        }

        public virtual bool CheckUpdate(DatabaseContext _ctx, FileInfo info)
        {
            var updateXls = _ctx.UpdateXls.AsNoTracking().FirstOrDefault(p => p.FileName == GetFileNameByAttribute());
            string hash = GenerateContentHash(info);
            return updateXls == null || updateXls.Hash != hash;
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
        /// Aggiorna la tabella UpdateXls per i successivi confronti
        /// </summary>
        /// <param name="_ctx"></param>
        public void UpdateEntity(DatabaseContext _ctx, FileInfo info)
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
        /// Tramite l'attributo FileXlsImportAttribute restituisce il noem del file 
        /// </summary>
        /// <returns></returns>
        public string GetFileNameByAttribute()
        {
            var attr = this.GetType().GetCustomAttribute<FileXlsImportAttribute>();
            return attr?.Filename;
        }

        public FileInfo GetFileInfo(string searchFile)
        {
            string folderPath = @"Resources";


            // Crea un oggetto DirectoryInfo per la cartella specificata
            DirectoryInfo dInfo = new DirectoryInfo(folderPath);

            // Cerca il file specificato e ottieni il suo FileInfo
            // Il metodo GetFiles() può accettare un pattern di ricerca (es. "*.txt")
            // e noi lo filtriamo ulteriormente usando Linq per trovare il file esatto.
            FileInfo[] matchingFiles = dInfo.GetFiles(searchFile + ".xlsx");

            if (matchingFiles.Length > 0)
            {
                // Ottiene il primo file corrispondente (se ne esistono più di uno)
                FileInfo fileInfo = matchingFiles[0];
                return fileInfo;
            }
            return null;

        }

        public string pathService()
        {
            return Directory.GetCurrentDirectory() + "/logs/SyncTables.txt";
        }

        /// <summary>
        /// Utilizza l'algoritmo SHA256 per criptare la stringa ricevuta come parametro e restituirla al chiamante
        /// </summary>
        /// <param name="value">Stringa criptare</param>
        /// <returns>Restituisce la stringa passata come parametro criptata con l'algoritmo SHA256</returns>
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

        
    }
}

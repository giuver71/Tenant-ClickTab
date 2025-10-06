using ClickTab.Core.DAL.Context;
using ClickTab.Core.EntityService.Generics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClickTab.Core.HelperService
{
    public class DatabaseService
    {
        private UserService _userService;

        public DatabaseService(UserService userService)
        {
            _userService = userService;
        }

        private DbContext ResolveMigrationContext(DbProvider provider, string connectionString)
        {
            DbContext context = null;
            switch (provider)
            {
                case DbProvider.SQLServer:
                    context = new MSSQL_DbContext(connectionString, provider);
                    break;

                case DbProvider.MySql:
                    context = new MySQL_DbContext(connectionString, provider);
                    break;
            }

            return context;
        }

        
        public void ApplyMigrations(DbProvider provider, string connectionString, bool? syncDefaultData = null)
        {
            DbContext context = ResolveMigrationContext(provider, connectionString);

            //Verifica se il DB esiste già e memorizza il risultato in una variabile
            //Servirà per fare in modo di predisporre gli eventuali dati di default se il DB risulta creato nuovo
            bool dbExists = (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

            var migrationAssembly = context.Database.GetService<IMigrationsAssembly>();
            var historyRepository = context.Database.GetService<IHistoryRepository>();
            var allContextMigrations = migrationAssembly.Migrations.Keys;
            var appliedContextMigrations = historyRepository.GetAppliedMigrations().Select(r => r.MigrationId);
            var pendingContextMigrations = allContextMigrations.Except(appliedContextMigrations);

            IMigrator migrator = context.Database.GetService<IMigrator>();
            if (pendingContextMigrations != null)
            {
                foreach (string migrationName in pendingContextMigrations)
                    migrator.Migrate(migrationName);
            }

            //Se è stata richiesta la sincronizzazione dei dati di default per il DB allora richiama la funzione che dovrà occuparsi di predisporre i dati di default
            //Alla funzione viene anche passato un parametro che permette di sapere se il DB è stato creato per la prima volta oppure no
            if (syncDefaultData.HasValue && syncDefaultData.Value == true)
                CreateDatabaseDefaultData(!dbExists);
        }

        //private DbContext ResolveMigrationContext(DbProvider provider, string connectionString)
        //{
        //    DbContext context = null;
        //    switch (provider)
        //    {
        //        case DbProvider.SQLServer:
        //            context = new MSSQL_DbContext(connectionString, provider);
        //            break;

        //        case DbProvider.MySql:
        //            context = new MySQL_DbContext(connectionString, provider);
        //            break;
        //    }

        //    return context;
        //}

        /// <summary>
        /// Contiene tutta la logica necessaria per predisporre i dati di default da inserire nel DB
        /// di sistema
        /// </summary>
        /// <param name="isFirstCreation">Se TRUE significa che il DB è nuovo ed è stato appena creato</param>
        private void CreateDatabaseDefaultData(bool isFirstCreation)
        {
            //Se il DB è stato appena creato allora crea l'utenza di default per accedere
            if (isFirstCreation)
                _userService.CreateAdminUser();
        }
    }
}

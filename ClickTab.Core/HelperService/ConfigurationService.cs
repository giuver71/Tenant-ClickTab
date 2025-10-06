using System;
using System.Collections.Generic;
using System.Text;

namespace ClickTab.Core.HelperService
{
    public class ConfigurationService
    {
        public bool IsDevelopment { get; set; }
        public string AppName { get; set; }
        public string ConnectionString { get; set; }
        public bool SyncDefaultData { get; set; }
        public string SyncVersionData { get; set; }
        public DbProvider DbContextProvider { get; set; }
        public FileStorageMode FileStorageMode { get; set; }
        public string BaseDomainClient { get; set; }
        public string BaseDomainServer { get; set; }
        public string[] AdditionalCorsOrigins { get; set; }
        public string ContentRootPath { get; set; }

        #region Proprietà per configurazione domini app mobile (se presenti)

        public string BaseDomainMobileAppANDROID { get; set; }
        public string BaseDomainMobileAppIOS { get; set; }

        #endregion

        #region Proprietà per configurazione invio mail
        public string EmailSMTPClient { get; set; }
        public string EmailFrom { get; set; }
        public string EmailPassword { get; set; }
        public int EmailPortNumber { get; set; }
        public bool? EmailEnableSSL { get; set; }

        #endregion
    }

    public enum DbProvider
    {
        SQLServer = 1,
        MySql = 2
    }

    public enum FileStorageMode
    {
        ProjectFolder = 1
    }
}

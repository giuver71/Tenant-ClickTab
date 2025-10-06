using EQP.EFRepository.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.DAL.Models.Generics
{
    /// <summary>
    /// La classe seguente contiene le informazioni  di file XLSX utilizzati per l'Aggiornamento delle tabelle di sistema
    /// </summary>
    public class UpdateXls:IBaseEntity,IAuditEntity<int>
    {
        public int ID { get; set; }
        /// <summary>
        /// Nome senza estensione del file XLSX 
        /// Univoco nella tabella
        /// </summary>
        public string FileName { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Hash { get; set; }
        public int FK_InsertUser { get ; set ; }
        public DateTime InsertDate { get ; set ; }
        public int FK_UpdateUser { get ; set ; }
        public DateTime? UpdateDate { get ; set ; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.HelperService.XLSUpdateHelper
{

    public class FileXlsImportAttribute :Attribute
    {
        public FileXlsImportAttribute(string filename)
        {
            Filename = filename;
        }
        /// <summary>
        /// Nome del File XLS relativo alla tabella da aggirnare
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Qui aggiungere i nomi dei Files relativi ad eventuali dipendenze da aggiornare obbligatoriamente prima di aggiornare la tabella relativa al filename
        /// Si usa per quelle tabelle che hanno refere4nze
        /// Esempio Paragraphs avra come Dependencies Chapters
        /// In caso di referenze multiple utilizzare il carattere | per lo split
        /// </summary>
        public string[] Dependencies { get; set; }
    }
}

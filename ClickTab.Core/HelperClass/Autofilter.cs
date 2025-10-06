using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTab.Core.HelperClass
{
    public class AutoFilter
    {
        /// <summary>
        /// Identifica la key della query da ricercare nel dictionary delle query 
        /// </summary>
        public string QueryKey { get; set; }


        /// <summary>
        /// Identifica la key della query da ricercare nel dictionary delle query  da eseguire al blur dell'oggetto client
        /// </summary>
        public string OnBlurQueryKey { get; set; }

        /// <summary>
        /// Collection dei parametri da valorizzare nella query
        /// </summary>
        public Dictionary<string, object> Params { get; set; }


        /// <summary>
        /// Nome del parametro utilizzato nella query x effetture la ricerca interattiva
        /// </summary>
        public string AutoFilterParameterName { get; set; }


        /// <summary>
        /// valore in base al quale viene eseguita la ricerca interattiva
        /// </summary>
        public string ValueSearch { get; set; }

        /// <summary>
        /// Aggiunge la clasuola Distinct alla SELECT
        /// </summary>
        public bool IsDistinctSelect { get; set; }




    }
}

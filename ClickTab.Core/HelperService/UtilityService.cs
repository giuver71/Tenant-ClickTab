using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ClickTab.Core.HelperService
{
    public static class UtilityService
    {
        /// <summary>
        /// Sostituisce i placeholder nel testo con i valori ricevuti in input.
        /// Recupera le chiavi del dizionario ed esegue la replace nel testo cercando i placeholders con lo stesso nome racchiusi 
        /// tra i valori passati in leftDelimiter e rightDelimiter.
        /// </summary>
        /// <param name="text">Testo in cui rimpiazzare i placeholder</param>
        /// <param name="parameters">Dizionario contenente i valori dei placeholder nel test</param>
        /// <param name="leftDelimiter">Caratteri che limitano a sinistra i placeholder nel testo. Di default è impostato a "{" (per placeholder del tipo: "{Placeholder}")</param>
        /// <param name="rightDelimiter">Caratteri che limitano a destra i placeholder nel testo. Di default è impostato a "}" (per placeholder del tipo: "{Placeholder}")</param>
        /// <returns></returns>
        public static string ReplaceTextPlaceholders(string text, Dictionary<string, object> parameters, string leftDelimiter = "{", string rightDelimiter = "}")
        {
            // Effettua un cotnrollo sul tipo dei parametri passati, se è una data viene formattata in base alla lingua in sessione.
            List<string> dictionaryKeys = parameters != null ? new List<string>(parameters.Keys) : new List<string>();
            foreach (string key in dictionaryKeys)
            {
                string placeholder = leftDelimiter + key + rightDelimiter;
                if (!parameters.ContainsKey(key) || parameters[key] == null)
                    text = text.Replace(placeholder, "");
                else
                    text = text.Replace(placeholder, parameters[key] is DateTime ? DateTime.Parse(parameters[key].ToString()).ToString("d", new CultureInfo("it-IT")) : (string)parameters[key]);
            }

            return text;
        }
    }
}

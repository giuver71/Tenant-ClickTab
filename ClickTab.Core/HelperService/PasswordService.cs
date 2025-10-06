using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ClickTab.Core.HelperService
{
    public class PasswordService
    {
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
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }

        /// <summary>
        /// Genera una password casuale
        /// </summary>
        /// <returns></returns>
        public string MakePassword()
        {
            string password = Guid.NewGuid().ToString().Substring(0, 8);
            return System.Text.RegularExpressions.Regex.Replace(password, @"[^a-zA-Z0-9]", m => "9");
        }
    }
}

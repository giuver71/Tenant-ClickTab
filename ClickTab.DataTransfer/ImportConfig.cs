using ClickTab.Core.DAL.Models.Tables;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;

namespace ClickTab.DataTransfer
{
    public static class ImportConfig
    {
        const string keyConnection = "AIELLO";
        static SqlConnection cnn;
        public static Dictionary<string, string> Connections = new Dictionary<string, string>();
        static ImportConfig()
        {
            AddConnections();

            string cnnStr = Connections[keyConnection];
            cnn = new SqlConnection(cnnStr);

        }

        private static void AddConnections()
        {
            Connections.Add("AIELLO", "Data Source=w81;Initial Catalog=GetabSql;User ID=sa;Password=71c12d086;TrustServerCertificate=true;MultipleActiveResultSets=true;Connection Timeout=600");
        }

        /// <summary>
        /// Inizializza la connessione e carica i gruppi
        /// </summary>
        /// 
        public static List<Category> ToMappingGruppi(string keyConnection)
        {
            if (!Connections.ContainsKey(keyConnection))
                throw new ArgumentException($"Connection key '{keyConnection}' not found.");

            string cnnStr = Connections[keyConnection];

            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Gruppi Where Len(Ltrim(CodGru))>0", cnn);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            var mapGruppi = new Dictionary<string, string>(){
                { "CodGru", "Code" },
                { "DesGru", "Description" },
                { "AggioGr", "Fee" },
                { "Fiscale", "IsFiscal" },
                { "Reparto", "Department" },
                { "Negativo", "Negative" },
                { "CalcP", "CalcP" }
            };

                return DataMapper.MapTableToList<Category>(dt, mapGruppi);
            }
        }


        public static List<SubCategory> ToMappingSottoGruppi(string keyConnection)
        {
            if (!Connections.ContainsKey(keyConnection))
                throw new ArgumentException($"Connection key '{keyConnection}' not found.");

            string cnnStr = Connections[keyConnection];

            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM SottoGruppi Where Len(Ltrim(CodSot))>0", cnn);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                var mapGruppi = new Dictionary<string, string>(){
                { "CodSot", "Code" },
                { "DeSot", "Description" },
            };

                return DataMapper.MapTableToList<SubCategory>(dt, mapGruppi);
            }
        }



        public static class DataMapper
        {
            /// <summary>
            /// Mappa una DataRow in un oggetto del tipo T, usando una mappa di colonne/proprietà.
            /// </summary>
            public static T MapRowToObject<T>(DataRow row, Dictionary<string, string> map) where T : new()
            {
                if (row == null)
                    throw new ArgumentNullException(nameof(row));
                if (map == null)
                    throw new ArgumentNullException(nameof(map));

                T obj = new T();
                Type type = typeof(T);

                foreach (var kvp in map)
                {
                    string columnName = kvp.Key;
                    string propertyName = kvp.Value;

                    if (!row.Table.Columns.Contains(columnName))
                        continue;

                    object value = row[columnName];
                    if (value == DBNull.Value)
                        continue;

                    var prop = type.GetProperty(propertyName);
                    if (prop != null && prop.CanWrite)
                    {
                        try
                        {
                            // Conversione tipo-safe
                            object convertedValue = Convert.ChangeType(value, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                            prop.SetValue(obj, convertedValue);
                        }
                        catch
                        {
                            // In caso di tipo incompatibile (es. string -> bool)
                            // puoi gestire log o fallback
                        }
                    }
                }

                return obj;
            }

            /// <summary>
            /// Mappa un intero DataTable in una lista di oggetti del tipo T.
            /// </summary>
            public static List<T> MapTableToList<T>(DataTable dt, Dictionary<string, string> map) where T : new()
            {
                List<T> list = new List<T>();

                foreach (DataRow row in dt.Rows)
                {
                    T obj = MapRowToObject<T>(row, map);
                    list.Add(obj);
                }

                return list;
            }
        }


    }


}

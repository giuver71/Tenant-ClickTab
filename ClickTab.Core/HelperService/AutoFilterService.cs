using Newtonsoft.Json;
using ClickTab.Core.Extensions;
using ClickTab.Core.HelperClass;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ClickTab.Core.HelperService
{
    public class AutoFilterService
    {
        private Dictionary<string, string> _queriesDictionary = new Dictionary<string, string>();
        private ConfigurationService _configurationService;

      
        public AutoFilterService(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
            this.BuildQueryDictionary();
        }

        private void BuildQueryDictionary()
        {

            #region ESENZIONI
            _queriesDictionary.Add("EXEMPTION", "Select Id,Code,Description from Exemptions WHERE Description LIKE '%' + @searchdescription + '%' OR Code LIKE '%' + @searchdescription + '%'");
            #endregion

            #region DIAGNOSI E INTERVENTI
            _queriesDictionary.Add("DIAGNOSIS", "Select Id,Code,Description from Icd9ds WHERE Description LIKE @searchdescription + '%' OR Code LIKE '%' + @searchdescription + '%'");
            _queriesDictionary.Add("PROCEDURES", "Select Id,Code,Description from Icd9cmis WHERE Description LIKE @searchdescription + '%' OR Code LIKE '%' + @searchdescription + '%'");
            #endregion

            #region MEDICI DI BASE E REPARTO
            _queriesDictionary.Add("MMG", "Select Id,Code,Denomination from doctors WHERE Denomination LIKE '%' + @searchdescription + '%' OR Code LIKE '%' + @searchdescription + '%'");
            _queriesDictionary.Add("DOCTORS_DEPARTMENT", "Select ID,Code,Name from DoctorsDepartments WHERE name LIKE '%' + @searchdescription + '%' OR Code LIKE '%' + @searchdescription + '%'");
            #endregion

            #region COMUNI E REGIONI E STATI
            _queriesDictionary.Add("COMUNAS", "Select Municipalities.Id,Municipalities.Code,Municipalities.Denomination,Municipalities.CatastalCode,Municipalities.FK_Region,Municipalities.FK_Location,Regions.Name as RegionName,Locations.Denomination as LocationName from Municipalities Inner Join Regions ON Municipalities.FK_Region=Regions.ID LEFT Join Locations ON Municipalities.FK_Location=Locations.ID  WHERE Municipalities.Denomination LIKE '%' + @searchdescription + '%' OR Municipalities.Code LIKE '%' + @searchdescription + '%'");

            _queriesDictionary.Add("REGIONS", "Select Id,Code,Name from regions WHERE Name LIKE '%' + @searchdescription + '%' OR Code LIKE '%' + @searchdescription + '%'");

            _queriesDictionary.Add("CITIZEN_SHIP", "Select Id,Code,Denomination from Citizenships WHERE Denomination LIKE '%' + @searchdescription + '%' OR Code LIKE '%' + @searchdescription + '%'");

            _queriesDictionary.Add("LOCATIONS", "Select Id,Code,Denomination from Locations WHERE Denomination LIKE '%' + @searchdescription + '%' OR Code LIKE '%' + @searchdescription + '%'");

            #endregion

            #region MAGAZZINO
            _queriesDictionary.Add("ARTICLES", "Select Articles.ID,Articles.Code,Articles.Description,Articles.FK_Facilitie,Articles.FK_VatTable,Articles.FK_MeasureUnit, MeasureUnits.Description as UM, VatTables.Description VatDescription from Articles Inner Join MeasureUnits on Articles.FK_MeasureUnit = MeasureUnits.ID    inner join VatTables on Articles.FK_VatTable = VatTables.ID WHERE Articles.Description LIKE '%' + @searchdescription + '%'");
            #endregion


}

        public string GetQuery(string key)
        {
            if (_queriesDictionary.ContainsKey(key))
            {
                return _queriesDictionary[key];
            }
            return null;
        }

        public string RunQuery(string query, AutoFilter autoFilter)
        {
            SqlConnection connection = new SqlConnection(_configurationService.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, connection);

            // Valorizzo il parametr, se esiste, derivante dagli input della casella dell'Autofilter
            if (!string.IsNullOrEmpty(autoFilter.AutoFilterParameterName))
            {
                cmd.Parameters.AddWithValueNullable(autoFilter.AutoFilterParameterName, autoFilter.ValueSearch);
            }
            if (autoFilter.Params != null && autoFilter.Params.Count > 0)
            {
                foreach (KeyValuePair<string, object> item in autoFilter.Params)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);

                }
            }
            connection.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);
            connection.Close();
            string result = dt.Rows.Count > 0 ? JsonConvert.SerializeObject(dt) : null;
            return result;
        }

    }
}

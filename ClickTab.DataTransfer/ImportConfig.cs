using System.Runtime.CompilerServices;

namespace ClickTab.DataTransfer
{
    public static class ImportConfig
    {
        public static Dictionary<string, string> Connections = new Dictionary<string, string>();
        static ImportConfig()
        {
            AddConnections();
        }

        private static void AddConnections()
        {
            Connections.Add("AIELLO", "Data Source=w81;Initial Catalog=GetabSql;User ID=sa;Password=71c12d086;TrustServerCertificate=true;MultipleActiveResultSets=true;Connection Timeout=600");
        }
    }

   
}

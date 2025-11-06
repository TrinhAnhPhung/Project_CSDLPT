using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace QL_HP_DT_SV.Utils
{
    internal class Helpers
    {
        public static string BuildSqlAuthConnection(string user, string pass, string site = "Site1")
        {
            string configName = site == "Site1" ? "MyDB_Site1" 
                              : site == "Site2" ? "MyDB_Site2" 
                              : "MyDB_Site3";
            
            var baseConn = ConfigurationManager.ConnectionStrings[configName]?.ConnectionString;
            if (baseConn == null)
            {
                throw new Exception($"Connection string for {site} not found in App.config");
            }
            
            var sb = new SqlConnectionStringBuilder(baseConn)
            {
                IntegratedSecurity = false,  // chuyển sang SQL Auth
                UserID = user,
                Password = pass,
                // đảm bảo Database/Server giữ nguyên từ config
            };
            return sb.ToString();
        }
    }
}

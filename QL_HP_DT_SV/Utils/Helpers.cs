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
        public static string BuildSqlAuthConnection(string user, string pass)
        {
            var baseConn = ConfigurationManager.ConnectionStrings["MyDB_Site1"].ConnectionString;
            var sb = new SqlConnectionStringBuilder(baseConn)
            {
                IntegratedSecurity = false,  // chuyển sang SQL Auth
                UserID = user,
                Password = pass,
                // đảm bảo Database/Server giữ nguyên từ MyDB
            };
            return sb.ToString();
        }
    }
}

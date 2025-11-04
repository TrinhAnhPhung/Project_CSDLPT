using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_HP_DT_SV.Utils
{
    internal class Session
    {
        public static string ConnectionString { get; set; }
        public static string CurrentUserName { get; set; }
        public static string CurrentUserRole { get; set; }
        public static bool IsAdmin { get; set; }
        
        public static void Clear()
        {
            ConnectionString = null;
            CurrentUserName = null;
            CurrentUserRole = null;
            IsAdmin = false;
        }
    }
}

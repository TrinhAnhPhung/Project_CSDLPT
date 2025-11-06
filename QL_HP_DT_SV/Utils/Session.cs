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
        public static string CurrentSite { get; set; } // "Site1", "Site2", "Site3"
        public static string CurrentMaKhoa { get; set; } // Mã khoa hiện tại
        public static string CurrentTenKhoa { get; set; } // Tên khoa hiện tại
        
        public static void Clear()
        {
            ConnectionString = null;
            CurrentUserName = null;
            CurrentUserRole = null;
            IsAdmin = false;
            CurrentSite = null;
            CurrentMaKhoa = null;
            CurrentTenKhoa = null;
        }
    }
}

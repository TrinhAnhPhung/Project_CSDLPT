using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QL_HP_DT_SV.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "Student";
        public bool IsActive { get; set; } = true;
    }
}

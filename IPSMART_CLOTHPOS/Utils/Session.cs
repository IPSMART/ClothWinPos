using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSMART_CLOTHPOS.Utils
{
    public static class Session
    {
        public static string ModuleCode { get; set; } = "FIN";
        public static string Modcd { get; set; } = "F";
        public static string UserId { get; set; }
        public static string UserName { get; set; }
        public static string UserType { get; set; }        
        public static string Compcd { get; set; }
        public static string Loccd { get; set; }
    }
}

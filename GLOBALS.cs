using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _202223_bbs_projekt_kasse
{
    internal class GLOBALS
    {
        public static string COMM_PORT_SCN { get; set; } = "COM1";
        public static string COMM_PORT_DISP { get; set; }
        public static string SQL_USER { get; set; }
        public static string SQL_PASSWORD { get; set; }
        public static string SQL_IP { get; set; }
        public static string SQL_DB { get; set; }
        public static List<Produkt> currentBon { get; set; } 
    }
}

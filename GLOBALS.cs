﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml.Schema;
using System.IO.Ports;

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
        public static ItemCollection currentBon { get; set; } 
        public static double Total { get; set; }
        public static SerialPort SPScan { get; set; }
        public static SerialPort SPDisp { get; set; }

    }
}

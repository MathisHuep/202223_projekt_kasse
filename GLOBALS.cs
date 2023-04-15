using System;
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
        //Globale Variablen zur Globalen Übergabe von Werten

        //COM Port Konfiguration für Scanner
        public static string COM_PORT_SCN { get; set; } = "COM1";
        //COM Port Konfiguration für Display
        public static string COM_PORT_DISP { get; set; }
        //User SQL
        public static string SQL_USER { get; set; }
        //Passwort SQL user
        public static string SQL_PASSWORD { get; set; }
        //SQL Server IP
        public static string SQL_IP { get; set; }
        //SQL Datenbank
        public static string SQL_DB { get; set; }
        //Übergabe des Bons an nachfolgende Fenster
        public static ItemCollection currentBon { get; set; } 
        //Übergabe des Gesamtpreises eines Einkaufs
        public static double Total { get; set; }
        //Übergabe für Globalen Zugriff auf Serial Port für Scanner 
        public static SerialPort SPScan { get; set; }
        //Übergabe für Globalen Zugriff auf Serial Port für Display
        public static SerialPort SPDisp { get; set; }
        //Übergibt ob der Bezahlvorgang abgeschlossen ist
        public static bool paymentSuccessful { get; set; }

    }
}

using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _202223_bbs_projekt_kasse
{
    /// <summary>
    /// Interaktionslogik für WindowSQLlogin.xaml
    /// Fenster zur erneuten Bestimmung der Loginparameter für SQL Verbindung, bei gescheiterter Verbindungsaufnahme zur Datenbank
    /// </summary>
    public partial class WindowSQLlogin : Window
    {
        public WindowSQLlogin()
        {
            InitializeComponent();
            //Übernahme der Loginparameter zur erneuten Anzeige der zuerst eingegebenen Werte
            sqliplogin.Text = GLOBALS.SQL_IP;
            sqluserlogin.Text = GLOBALS.SQL_USER;
            sqlpasslogin.Password = GLOBALS.SQL_PASSWORD;
            sqldblogin.Text = GLOBALS.SQL_DB;
        }

        //Funktionalität Confirm Knopf
        private void sqlloginconfirm_Click(object sender, RoutedEventArgs e)
        {
            //Geänderte Variablen werden wieder in globale Variablen geschrieben
            GLOBALS.SQL_IP = sqliplogin.Text;
            GLOBALS.SQL_USER = sqluserlogin.Text;
            GLOBALS.SQL_PASSWORD = sqlpasslogin.Password;
            GLOBALS.SQL_DB = sqldblogin.Text;
            //WindowSQLlogin wird geschlossen
            this.Close();
        }
    }
}

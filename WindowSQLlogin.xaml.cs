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
    /// Fenster zur erneuten bestimmung der Login Parameter für SQL Verbindung, bei gescheiterter Verbindungsaufnahme zur Datenbank
    /// </summary>
    public partial class WindowSQLlogin : Window
    {
        public WindowSQLlogin()
        {
            InitializeComponent();
            //Übernahme der Login Parameter zur erneuten Anzeige der zuerst eingegebenen Wert
            sqliplogin.Text = GLOBALS.SQL_IP;
            sqluserlogin.Text = GLOBALS.SQL_USER;
            sqlpasslogin.Password = GLOBALS.SQL_PASSWORD;
            sqldblogin.Text = GLOBALS.SQL_DB;
        }

        //Funktionalität Confirm Knopf
        private void sqlloginconfirm_Click(object sender, RoutedEventArgs e)
        {
            //Geänderte Variabeln werden wieder in Globale Variablen Geschreiben
            GLOBALS.SQL_IP = sqliplogin.Text;
            GLOBALS.SQL_USER = sqluserlogin.Text;
            GLOBALS.SQL_PASSWORD = sqlpasslogin.Password;
            GLOBALS.SQL_DB = sqldblogin.Text;
            //WindowSQLlogin wird geschlossen
            this.Close();
        }
    }
}

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
    /// </summary>
    public partial class WindowSQLlogin : Window
    {
        public WindowSQLlogin()
        {
            InitializeComponent();
            sqliplogin.Text = GLOBALS.SQL_IP;
            sqluserlogin.Text = GLOBALS.SQL_USER;
            sqlpasslogin.Password = GLOBALS.SQL_PASSWORD;
            sqldblogin.Text = GLOBALS.SQL_DB;
        }
        private void sqlloginconfirm_Click(object sender, RoutedEventArgs e)
        {
            GLOBALS.SQL_IP = sqliplogin.Text;
            GLOBALS.SQL_USER = sqluserlogin.Text;
            GLOBALS.SQL_PASSWORD = sqlpasslogin.Password;
            GLOBALS.SQL_DB = sqldblogin.Text;
            this.Close();
        }
    }
}

﻿using System;
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
    /// Interaktionslogik für WindowOptions.xaml
    /// </summary>
    public partial class WindowOptions : Window
    {
        public WindowOptions()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GLOBALS.SQL_IP = sqlip.Text;
            GLOBALS.SQL_USER = sqluser.Text;
            GLOBALS.SQL_PASSWORD = sqlpass.Password;
            GLOBALS.SQL_DB = sqldb.Text;
            GLOBALS.COMM_PORT_DISP = commdisp.Text;
            GLOBALS.COMM_PORT_SCN = commscn.Text;
            this.Close();
        }
    }
}
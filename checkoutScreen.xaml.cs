using Org.BouncyCastle.Math;
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
    /// Interaction logic for checkoutScreen.xaml
    /// </summary>
    public partial class checkoutScreen : Window
    {
        public checkoutScreen()
        {
            InitializeComponent();
            foreach (var item in GLOBALS.currentBon)
            {
                bon_list.Items.Add(item);
            }
            
            /*foreach (var item in GLOBALS.currentBon) 
            {
                bon_list.Items.Add(item);
                bon_list_total.Text = Convert.ToString(Math.Round(Convert.ToDouble(bon_list_total.Text) + item.Preis, 3));
            }*/
        }

        private void numpad_but_1_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "1";
        }

        private void numpad_but_2_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "2";
        }

        private void numpad_but_3_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "3";
        }

        private void numpad_but_4_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "4";
        }

        private void numpad_but_5_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "5";
        }

        private void numpad_but_6_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "6";
        }

        private void numpad_but_7_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "7";
        }

        private void numpad_but_8_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "8";
        }

        private void numpad_but_9_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "9";
        }

        private void numpad_but_decimal_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += ".";
        }

        private void numpad_but_0_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "0";
        }

        private void numpad_but_double0_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "00";
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if(numpad_output1.Content.ToString().Length != 0)
            {
                numpad_output1.Content = numpad_output1.Content.ToString().Remove(numpad_output1.Content.ToString().Length - 1);
            }
            
        }
    }
}

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
                bon_list_total.Text = Convert.ToString(Math.Round(Convert.ToDouble(bon_list_total.Text) + item.Preis, 3));
            }
        }

        private void numpad_but_1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void numpad_but_2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void numpad_but_3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void numpad_but_4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void numpad_but_5_Click(object sender, RoutedEventArgs e)
        {

        }

        private void numpad_but_6_Click(object sender, RoutedEventArgs e)
        {

        }

        private void numpad_but_7_Click(object sender, RoutedEventArgs e)
        {

        }

        private void numpad_but_8_Click(object sender, RoutedEventArgs e)
        {

        }

        private void numpad_but_9_Click(object sender, RoutedEventArgs e)
        {

        }

        private void numpad_but_clear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void numpad_but_0_Click(object sender, RoutedEventArgs e)
        {

        }

        private void numpad_but_ent_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

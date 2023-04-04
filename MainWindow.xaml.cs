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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _202223_bbs_projekt_kasse
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void numpad_but_div_Click(object sender, RoutedEventArgs e)
        {

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

        private void numpad_but_0_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "0";
        }

        private void numpad_but_ent_Click(object sender, RoutedEventArgs e)
        {
            bon_list.Items.Add(numpad_output1.Content);
            numpad_output1.Content = null;
        }

        private void numpad_but_clear_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content = null;
        }
    }
}

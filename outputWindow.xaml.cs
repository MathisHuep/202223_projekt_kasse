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
    /// Interaktionslogik für outputWindow.xaml
    /// Fenster zur Ausgabe von Fehlermeldungen
    /// </summary>
    public partial class outputWindow : Window
    {
        public outputWindow(string output)
        {
            InitializeComponent();
            //Fehlermeldung wird angezeigt
            Output.Text= output;
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            //Nach bestätigen wird Fenster geschlossen
            this.Close();
        }
    }
}

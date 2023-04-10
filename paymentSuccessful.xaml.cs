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
using System.Threading;

namespace _202223_bbs_projekt_kasse
{
    /// <summary>
    /// Interaction logic for paymentSuccessful.xaml
    /// Fenster zur Anzeige, dass Bezahlprozess abgeschlossen ist
    /// </summary>
    public partial class paymentSuccessful : Window
    {
        public paymentSuccessful()
        {
            InitializeComponent();
        }

        //Funktionalität Bestätigen Knopf
        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            //paymentSuccessful wird geschlossen
            this.Close();
        }
    }
}

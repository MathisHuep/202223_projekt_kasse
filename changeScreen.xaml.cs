using Org.BouncyCastle.Asn1.Cmp;
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
using System.Globalization;

namespace _202223_bbs_projekt_kasse
{
    /// <summary>
    /// Interaction logic for changeScreen.xaml
    /// Fenster zur Bestimmung und Anzeige für Wechselgeld 
    /// </summary>
    public partial class changeScreen : Window
    {
        public changeScreen(double mustBePaid, double Paid)
        {
            InitializeComponent();
            //Berechnung des Rückgeldes
            double changeAmount = Paid - mustBePaid;
            //Anzeige von Rückgeld in TextBlock Change
            change.Text = Convert.ToString(Math.Round(changeAmount, 3));
            //Anzeige von gegebenem Geld in TextBlock cash_given
            cash_given.Text =Convert.ToString(Paid);
            //Anzeige des zu begleichenden Betrags in TextBlock bon_list_total
            bon_list_total.Text = Convert.ToString(mustBePaid);
        }

        //Funktionalität Fertig Knopf 
        private void confirmChangeClick(object sender, RoutedEventArgs e)
        {
            //Bezahlvorgang wird Global als erfolgreich gesetzt
            GLOBALS.paymentSuccessful = true;
            //changeScreen wird geschlossen
            this.Close();
        }
    }
}

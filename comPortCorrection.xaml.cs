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
    /// Interaktionslogik für comPortCorrection.xaml
    /// Fenster zur Berrichtigung der Com Port Eingabe
    /// </summary>
    public partial class comPortCorrection : Window
    {
        //Variable zum hervorheben des falschen Com Port SoD: Sacnner or Display. 1:Scanner, 0:Display
        public comPortCorrection(bool SoD)
        {
            InitializeComponent();
            //Einsetzten der bisher verwendeten Parameter 
            comPortDisplayTextBox.Text = GLOBALS.COM_PORT_DISP;
            comPortScannerTextBox.Text = GLOBALS.COM_PORT_SCN;
            //Hervorheben des fehlerverursachenden Parameter
            if (SoD)
            {
                comPortScannerTextBox.Background = Brushes.Red;
            }
            else
            {
                comPortDisplayTextBox.Background = Brushes.Red;
            }
        }

        private void comPortCorrectionConfirm_Click(object sender, RoutedEventArgs e)
        {
            //Übernehmen der Änderungen in Globale Variablen
            GLOBALS.COM_PORT_DISP = comPortDisplayTextBox.Text;
            GLOBALS.COM_PORT_SCN = comPortScannerTextBox.Text;
            this.Close();
        }
    }
}

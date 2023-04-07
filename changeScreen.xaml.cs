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

namespace _202223_bbs_projekt_kasse
{
    /// <summary>
    /// Interaction logic for changeScreen.xaml
    /// </summary>
    public partial class changeScreen : Window
    {
        public changeScreen(double mustBePaid, double Paid)
        {
            InitializeComponent();
            double changeAmount = Paid - mustBePaid;
            change.Text = Convert.ToString(changeAmount);
            cash_given.Text =Convert.ToString(Paid);
            bon_list_total.Text = Convert.ToString(mustBePaid);
        }

        private void confirmChangeClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

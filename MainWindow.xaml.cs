using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace _202223_bbs_projekt_kasse
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {
        public string comm_port = GLOBALS.COMM_PORT_SCN;
        public int op_mode = 0;
        SerialPort SPScan = GLOBALS.SPScan;
        SerialPort SPDisp = GLOBALS.SPDisp;
        public MySqlConnection connection = null;

        public MainWindow()
        {
            InitializeComponent();
            bon_list_sum.Text = "0.00";
            bon_list_total.Text = "0.00";

            WindowOptions WinOpt = new WindowOptions();
            WinOpt.ShowDialog();

            Thread scanning = new Thread(scannerInput);
            Thread SQlquery = new Thread(Connect_to_SQL);
            Thread output = new Thread(displayOutput);
            SQlquery.Start();
            scanning.Start();
            output.Start();



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
            switch (op_mode)
            {
                case 0:
                    {
                        //Manuelle EAN eingabe
                        
                        numpad_output1.Content = null;
                        break;
                    }
                case 1:
                    {
                        //DIV
                        
                        
                        break;
                    }
                    
                default:
                    break;
            }
            op_mode = 0;
        }

        private void checkoutClick(object sender, RoutedEventArgs e)
        {
            GLOBALS.currentBon = bon_list.Items;
            GLOBALS.Total = Convert.ToDouble(bon_list_total.Text);
            SPDisp.Write("\x1B[2J");
            SPDisp.Write("\x1B[1;1H");
            SPDisp.Write("TOTAL");
            SPDisp.Write($"\x1B[2;{20 - (Convert.ToString(GLOBALS.Total).Length - 1)}H");
            SPDisp.Write(Convert.ToString(GLOBALS.Total));
            if (bon_list.Items.Count != 0)
            { 
                checkoutScreen chescr = new checkoutScreen();
                chescr.ShowDialog();
            }
            else
            { 
                //Fehlermeldung: Kein Item Gescannt!
            }
            
        }

        private void numpad_but_clear_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content = null;
        }
        public void scannerInput()
        {
            SPScan = new SerialPort(GLOBALS.COMM_PORT_SCN);
            SPScan.BaudRate = 9600;
            SPScan.Parity = Parity.Odd;
            SPScan.StopBits = StopBits.One;
            SPScan.DataBits = 8;
            SPScan.Handshake = Handshake.None;
            SPScan.RtsEnable = true;
            SPScan.DtrEnable = true;
            SPScan.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);
            
            try
            {
                //SPScan.Open();
            }
            catch (Exception)
            {
                //Fehlermeldung: Falscher COM Port!
                throw;
            }
            
        }

        public void displayOutput()
        {
            SPDisp = new SerialPort (GLOBALS.COMM_PORT_DISP);
            SPDisp.BaudRate = 9600;
            SPDisp.Parity = Parity.Odd;
            SPDisp.StopBits = StopBits.One;
            SPDisp.DataBits = 8;
            SPDisp.Handshake = Handshake.None;
            SPDisp.RtsEnable = true;
            SPDisp.DtrEnable = true;

            try
            {
                SPDisp.Open();
            }
            catch (Exception)
            {
                //Fehlermeldung: Falscher COM Port!
                throw;
            }
            SPDisp.Write("\x1BR02");
            SPDisp.Write("\x1B[0c");
        }

        public void displayOnDisplay(string produktName, float Preis)
        {
            SPDisp.Write("\x1B[2J");
            SPDisp.Write("\x1B[1;1H");
            SPDisp.Write(produktName);
            SPDisp.Write($"\x1B[2;{20 - (Convert.ToString(Preis).Length - 1)}H");
            SPDisp.Write(Convert.ToString(Preis));
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialDevice = sender as SerialPort;
            string barcode = serialDevice.ReadLine();
            barcode = barcode.Remove(barcode.Length - 1);

            string bezeichnung;
            float preis;
            string hersteller;

            string sql = $"SELECT Bezeichnung, Preis, Hersteller FROM produkte WHERE EAN = {barcode}";
            MySqlCommand command = new MySqlCommand(sql, connection);
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            
            
            MySqlDataReader reader = command.ExecuteReader();
            if(reader.Read())
            {
                bezeichnung = reader.GetString("Bezeichnung");
                preis = reader.GetFloat("Preis");
                hersteller = reader.GetString("Hersteller");
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    bon_list.Items.Add(new Produkt { Barcode = barcode, Hersteller = hersteller, Preis = preis, Name = bezeichnung });
                    bon_list_sum.Text = Convert.ToString(preis);
                    bon_list_total.Text = Convert.ToString(Math.Round(Convert.ToDouble(bon_list_total.Text) + preis, 3));
                }));
                displayOnDisplay(bezeichnung, preis);
                Debug.WriteLine("Produkt gefunden");               
            }
            else 
            {
                //Fehlermeldung no product found (Mathis)
                Debug.WriteLine($"Produktfehler/Nicht in der Datenbank/{connection.State}");
            }
            reader.Close();
            connection.Close();            
        }

        public void Connect_to_SQL() 
        {
            string connectionString = $"Server={GLOBALS.SQL_IP};Database={GLOBALS.SQL_DB};Uid={GLOBALS.SQL_USER}";
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                Debug.WriteLine($"SQL Connection failed. SQL connection state: {connection.State}");
                               
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    WindowSQLlogin winsqllog = new WindowSQLlogin();
                    winsqllog.ShowDialog();
                }));
                Connect_to_SQL();
            }
            Debug.WriteLine("SQL connection successful");
        }

        private void manualEANsubmit_Click(object sender, RoutedEventArgs e)
        {
            string barcode = manualEAN.Text;

            string bezeichnung;
            float preis;
            string hersteller;
            string sql = $"SELECT Bezeichnung, Preis, Hersteller FROM produkte WHERE EAN = {barcode}";
            MySqlCommand command = new MySqlCommand(sql, connection);
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            MySqlDataReader reader = command.ExecuteReader();
            
            if (manualEAN.Text != null)
            {

                if (reader.Read())
                {
                    bezeichnung = reader.GetString("Bezeichnung");
                    preis = reader.GetFloat("Preis");
                    hersteller = reader.GetString("Hersteller");
                    Application.Current.Dispatcher.Invoke(new Action(() => {
                        bon_list.Items.Add(new Produkt { Barcode = barcode, Hersteller = hersteller, Preis = preis, Name = bezeichnung });
                        bon_list_sum.Text = Convert.ToString(preis);
                        bon_list_total.Text = Convert.ToString(Math.Round(Convert.ToDouble(bon_list_total.Text) + preis, 3));
                    }));
                    displayOnDisplay(bezeichnung, preis);
                    Debug.WriteLine("Produkt gefunden");
                }
                else
                {
                    //Fehlermeldung no product found (Mathis)
                    Debug.WriteLine($"Produktfehler/Nicht in der Datenbank/{connection.State}");
                }
                reader.Close();
                connection.Close();
            }
        }
    }

    public class Produkt
    {
        public string Barcode { get; set; }
        public float Preis { get; set; }
        public string Hersteller { get; set; }
        public string Name { get; set; }
    }
}
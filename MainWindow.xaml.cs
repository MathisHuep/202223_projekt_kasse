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
        //Annahme von Globale Variabeln
        public string comm_port = GLOBALS.COMM_PORT_SCN;
        SerialPort SPScan = GLOBALS.SPScan;
        SerialPort SPDisp = GLOBALS.SPDisp;
        //Inizelisierung SQL Verbindung, vorerst mit null
        public MySqlConnection connection = null;

        public MainWindow()
        {
            InitializeComponent();
            //Setzung der Textfelde Bon_list_sum und bon_list_total auf 0
            bon_list_sum.Text = "0.00";
            bon_list_total.Text = "0.00";

            //Öffnung des Fenster für Inizeille einstellungen
            WindowOptions WinOpt = new WindowOptions();
            WinOpt.ShowDialog();

            //Erstellen und Starten der Prozesse für Scanner, SQL Zugriff und Display
            Thread scanning = new Thread(scannerInput);
            Thread SQlquery = new Thread(Connect_to_SQL);
            Thread output = new Thread(displayOutput);
            SQlquery.Start();
            scanning.Start();
            output.Start();



        }

        //Nachfolgend bis numpad_but_0_Click
        //Tasten Funktionalität des Tastenfelds
        //Tasten fügen zu oberem Textfeld korrespondierende Zahlen hinzu
        
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

        //Funktionalität fehlt
        private void numpad_but_ent_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content = null;
        }


        //Funktionalität checkout Butten
        private void checkoutClick(object sender, RoutedEventArgs e)
        {
            
            if (bon_list.Items.Count != 0)
            { 
                //Inhalt Bon_list wird an Globale Variable übegeben zur Wiedeverwendung
                GLOBALS.currentBon = bon_list.Items;
                //^^ gleiches mit Total Wert
                GLOBALS.Total = Convert.ToDouble(bon_list_total.Text);
                //Display wird Gelöscht
                SPDisp.Write("\x1B[2J");
                //Curser wird Positioniert [{Py};{Px}H
                SPDisp.Write("\x1B[1;1H");
                //Total wird anstlle auf Display ausgegebn
                SPDisp.Write("TOTAL");
                //Da Preis "Unten rechts" auf Display positioniert werden soll muss Preis länge bestimmt werden um Curser passend zu setztn [{Py};{Px}H
                //+3 damit Währung angezeigt werden kann
                SPDisp.Write($"\x1B[2;{20 - (Convert.ToString(GLOBALS.Total).Length + 3)}H");
                //Preis wird Ausgegeben
                SPDisp.Write(Convert.ToString(Math.Round(GLOBALS.Total, 3)));
                //Ausgabe Währung 
                SPDisp.Write(" EUR");
                //Serielle verbindung zu Display wird geschlossen
                SPDisp.Close();
                //checkoutScreen wird erstellt
                checkoutScreen chescr = new checkoutScreen();
                //checkoutScreen wird als Dialog angezeigt
                chescr.ShowDialog();
            }
            else
            { 
                //Fehlermeldung: Kein Item Gescannt!
            }
            
        }

        //Funktionalität "C" Butten
        //Leert den Inhalt von Output Feld (numpad_output1)
        private void numpad_but_clear_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content = null;
        }

        //Erstellung und Configuration von Serieller Schnittstelle von Scanner
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
                //öffnen der Schnitstelle 
                SPScan.Open();
            }
            catch (Exception)
            {
                //Fehlermeldung: Falscher COM Port!
                throw;
            }
            
        }

        //Erstellung und Configuration von Serieller Schnittstelle von Display 
        public void displayOutput()
        {
            //Konfiguration von Seriellem Port
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
                //öffnen der Schnitstelle 
                SPDisp.Open();
            }
            catch (Exception)
            {
                //Fehlermeldung: Falscher COM Port!
                throw;
            }
            //inizeiller Reset von Display
            SPDisp.Write("\x1BR02");
            SPDisp.Write("\x1B[0c");
        }

        public void displayOnDisplay(string produktName, float Preis)
        {
            //Neueröffung Serielle schnittstelle wenn geschlossen
            if(SPDisp.IsOpen == false)
            {
                SPDisp.Open();
            }
            //Display Reset
            SPDisp.Write("\x1B[2J");
            //Curser wird Positioniert [{Py};{Px}H
            SPDisp.Write("\x1B[1;1H");
            //Produkt Name wird anstlle auf Display ausgegebn
            SPDisp.Write(produktName);
            //Da Preis "Unten rechts" auf Display positioniert werden soll muss Preis länge bestimmt werden um Curser passend zu setztn [{Py};{Px}H
            //+3 damit Währung angezeigt werden kann
            SPDisp.Write($"\x1B[2;{20 - (Convert.ToString(Preis).Length + 3)}H");
            //ausgabe Preis von Produkt
            SPDisp.Write(Convert.ToString(Preis));
            //Ausgabe Währung
            SPDisp.Write(" EUR");
        }

        //EventHandler wenn Serielle Daten vom Scanner kommen
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialDevice = sender as SerialPort;
            //Entgegenname von Scanner input
            string barcode = serialDevice.ReadLine();
            //Entfernen von mitgesendetem Zeilenumbruch
            barcode = barcode.Remove(barcode.Length - 1);

            //Deklaration Variablen die aus Datenbank abgefragt werden
            string bezeichnung;
            float preis;
            string hersteller;

            //SQL query string mit Barcode
            string sql = $"SELECT Bezeichnung, Preis, Hersteller FROM produkte WHERE EAN = {barcode}";
            //Inizialisirung von MyMySqlCommand Instanz mit query und SQL verbindung
            MySqlCommand command = new MySqlCommand(sql, connection);
            //SQL verbindung wird neu eröffnet fals geschlossen
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            
            //Query wird Ausgeführt
            MySqlDataReader reader = command.ExecuteReader();
            //Wenn reader eine Rückgabe hat wird ausgeführt
            if(reader.Read())
            {
                //Zuweisung von variabeln aus SQL query
                bezeichnung = reader.GetString("Bezeichnung");
                preis = reader.GetFloat("Preis");
                hersteller = reader.GetString("Hersteller");
                //Eingriff in Window Thread per Dispatcher und Lambda Ausdruck
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    //Erstellung von Neuem Produkt Objekt und gelichzeitigem hinzufügen in bon_list
                    bon_list.Items.Add(new Produkt { Barcode = barcode, Hersteller = hersteller, Preis = preis, Name = bezeichnung });
                    //Produkt Preis wir auch im Summen Feld angezeigt
                    bon_list_sum.Text = Convert.ToString(preis);
                    //Preis wird auf den Totalen Pris drauf gerechnet
                    bon_list_total.Text = Convert.ToString(Math.Round(Convert.ToDouble(bon_list_total.Text) + preis, 3));
                }));
                //Anzeigen von gescanntem Produkt Auf display
                //hierfür übergaben an Funktion displayOnDisplay
                displayOnDisplay(bezeichnung, preis);
                Debug.WriteLine("Produkt gefunden");               
            }
            else 
            {
                //Fehlermeldung no product found (Mathis)
                Debug.WriteLine($"Produktfehler/Nicht in der Datenbank/{connection.State}");
            }
            //Reder wird geschlossen
            reader.Close();
            //Schließung fragwürdig
            connection.Close();            
        }

        //Verbindungs aufbau zu Datenbank
        public void Connect_to_SQL() 
        {
            //Verbindungs String für Datenbank verbindung mit eingesetztn Parametern
            string connectionString = $"Server={GLOBALS.SQL_IP};Database={GLOBALS.SQL_DB};Uid={GLOBALS.SQL_USER}";
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                //Bei gescheiterter Verbindung wird erneute eingabe von Datenbank Parametern gefordert
                Debug.WriteLine($"SQL Connection failed. SQL connection state: {connection.State}");
                
                //Eingriff in Window Thread per Dispatcher und Lambda ausdruck 
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    //Erstellen von WindowSQLlogin
                    //Erneute eingabe von SQL Parametern
                    WindowSQLlogin winsqllog = new WindowSQLlogin();
                    //Anzeige als Dialog
                    winsqllog.ShowDialog();
                }));
                //Selbst aufruf der Funktion zur ernueten überprüfung der Login Parameter der SQL Datenbank
                Connect_to_SQL();
            }
            Debug.WriteLine("SQL connection successful");
        }

        //Funktionalität Manuelle EAN eingabe
        //Kein Fail Safe
        private void manualEANsubmit_Click(object sender, RoutedEventArgs e)
        {
            //barcode wird von Mannuell eingegebenen Barcode zugewiesen
            string barcode = manualEAN.Text;

            //Deklaration Variablen die aus Datenbank abgefragt werden
            string bezeichnung;
            float preis;
            string hersteller;
            
            //SQL query string mit Barcode
            string sql = $"SELECT Bezeichnung, Preis, Hersteller FROM produkte WHERE EAN = {barcode}";
            //Inizialisirung von MyMySqlCommand Instanz mit query und SQL verbindung
            MySqlCommand command = new MySqlCommand(sql, connection);
            //SQL verbindung wird neu eröffnet fals geschlossen
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }


            MySqlDataReader reader = command.ExecuteReader();
            
            if (manualEAN.Text != null)
            {

                if (reader.Read())
                {
                    //Zuweisung von variabeln aus SQL query
                    bezeichnung = reader.GetString("Bezeichnung");
                    preis = reader.GetFloat("Preis");
                    hersteller = reader.GetString("Hersteller");
                    //Eingriff in Window Thread per Dispatcher und Lambda Ausdruck
                    Application.Current.Dispatcher.Invoke(new Action(() => {
                        //Erstellung von Neuem Produkt Objekt und gelichzeitigem hinzufügen in bon_list
                        bon_list.Items.Add(new Produkt { Barcode = barcode, Hersteller = hersteller, Preis = preis, Name = bezeichnung });
                        //Produkt Preis wir auch im Summen Feld angezeigt
                        bon_list_sum.Text = Convert.ToString(preis);
                        //Preis wird auf den Totalen Pris drauf gerechnet
                        double gesamtBetrag = Convert.ToDouble(bon_list_total.Text);
                        gesamtBetrag += preis;
                        //????
                        bon_list_total.Text = gesamtBetrag.ToString("F2");
                    }));
                    //Anzeigen von gescanntem Produkt Auf display
                    //hierfür übergaben an Funktion displayOnDisplay
                    displayOnDisplay(bezeichnung, preis);
                    Debug.WriteLine("Produkt gefunden");
                }
                else
                {
                    //Fehlermeldung no product found (Mathis)
                    Debug.WriteLine($"Produktfehler/Nicht in der Datenbank/{connection.State}");
                }
                //Reder wird geschlossen
                reader.Close();
                //Schließung fragwürdig
                connection.Close();
            }
        }
    }

    //Klasse für Produkt Definition
    public class Produkt
    {
        public string Barcode { get; set; }
        public float Preis { get; set; }
        public string Hersteller { get; set; }
        public string Name { get; set; }
    }
}
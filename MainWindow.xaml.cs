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
        //Annahme von globalen Variabeln
        public string comm_port = GLOBALS.COM_PORT_SCN;
        SerialPort SPScan = GLOBALS.SPScan;
        SerialPort SPDisp = GLOBALS.SPDisp;
        //Initalisierung der SQL Verbindung, vorerst mit null
        public MySqlConnection connection = null;

        public MainWindow()
        {
            InitializeComponent();
            //Anfängliches setzten des Bezahlvorgang auf false
            GLOBALS.paymentSuccessful = false;   
            //Setzen der Textfelder bon_list_sum und bon_list_total auf 0
            bon_list_sum.Text = "0.00";
            bon_list_total.Text = "0.00";

            //Öffnung des Fensters für initiale Einstellungen
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
        //Funktionalität der Tasten des Numpads
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
        

        //Funktionalität Checkout-Button
        private void checkoutClick(object sender, RoutedEventArgs e)
        {
            
            if (bon_list.Items.Count != 0)
            { 
                //Inhalt von bon_list wird zur Wiederverwendung an globale Variable übergeben
                GLOBALS.currentBon = bon_list.Items;
                //gleiches mit Total-Wert
                GLOBALS.Total = Convert.ToDouble(bon_list_total.Text);
                //Serial Port Verbindung überprüfen
                if (SPDisp.IsOpen == false)
                {
                    SPDisp.Open();
                }
                //Inhalt des Displays wird gelöscht
                SPDisp.Write("\x1B[2J");
                //Cursor wird positioniert [{Py};{Px}H
                SPDisp.Write("\x1B[1;1H");
                //Total wird an der ersten Stelle auf dem Display ausgegeben
                SPDisp.Write("TOTAL");
                //Da Preis "unten rechts" auf dem Display positioniert werden soll, muss die Länge vom Preis bestimmt werden, um Cursor passend zu setzen [{Py};{Px}H
                //+3 damit Währung angezeigt werden kann
                SPDisp.Write($"\x1B[2;{20 - (Convert.ToString(GLOBALS.Total).Length + 3)}H");
                //Preis wird ausgegeben
                SPDisp.Write(Convert.ToString(Math.Round(GLOBALS.Total, 3)));
                //Ausgabe Währung 
                SPDisp.Write(" EUR");
                //Serielle Verbindung zum Display wird geschlossen
                SPDisp.Close();
                //checkoutScreen wird erstellt
                checkoutScreen chescr = new checkoutScreen();
                //checkoutScreen wird als Dialog angezeigt
                chescr.ShowDialog();
                if (GLOBALS.paymentSuccessful)
                {
                    Reset();
                }
            }
            else
            {
                //Fehlermeldung: Kein Item Gescannt!
                outputWindow outwin = new outputWindow("Kein Item Gescannt!");
                outwin.ShowDialog();
            }
            
        }

        public void Reset() 
        {
            //Leeren der Bon Liste
            bon_list.Items.Clear();
            //Anfängliches setzten des Bezahlvorgang auf false
            GLOBALS.paymentSuccessful = false;
            //Setzen der Textfelder bon_list_sum und bon_list_total auf 0
            bon_list_sum.Text = "0.00";
            bon_list_total.Text = "0.00";
        }

        //Funktionalität "C" Button
        //Leert den Inhalt vom Output Feld (numpad_output1)
        private void numpad_but_clear_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content = null;
        }

        //Erstellen und Konfigurieren von serieller Schnittstelle vom Scanner
        public void scannerInput()
        {
            SPScan = new SerialPort(GLOBALS.COM_PORT_SCN);
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
                //Öffnen der Schnittstelle 
                SPScan.Open();
            }
            catch (Exception)
            {
                //Fehlermeldung: Falscher COM Port!
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    comPortCorrection comcor = new comPortCorrection(true);
                    comcor.ShowDialog();
                }));
                
                scannerInput();
            }
            
        }

        //Erstellen und Konfigurieren von serieller Schnittstelle vom Display 
        public void displayOutput()
        {
            //Konfiguration vom seriellen Port
            SPDisp = new SerialPort (GLOBALS.COM_PORT_DISP);
            SPDisp.BaudRate = 9600;
            SPDisp.Parity = Parity.Odd;
            SPDisp.StopBits = StopBits.One;
            SPDisp.DataBits = 8;
            SPDisp.Handshake = Handshake.None;
            SPDisp.RtsEnable = true;
            SPDisp.DtrEnable = true;

            try
            {
                //Öffnen der Schnittstelle 
                SPDisp.Open();
            }
            catch (Exception)
            {
                //Fehlermeldung: Falscher COM Port!
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    comPortCorrection comcor = new comPortCorrection(false);
                    comcor.ShowDialog();
                    displayOutput();
                }));
            }
            //initialer Reset des Displays
            SPDisp.Write("\x1BR02");
            SPDisp.Write("\x1B[0c");
            SPDisp.Write("\x1B[2J");
        }

        public void displayOnDisplay(string produktName, float Preis)
        {
            //Öffnen der seriellen Schnittstelle, falls sie geschlossen ist
            if(SPDisp.IsOpen == false)
            {
                SPDisp.Open();
            }
            //Display Reset
            SPDisp.Write("\x1B[2J");
            //Cursor wird positioniert [{Py};{Px}H
            SPDisp.Write("\x1B[1;1H");
            //Produktname wird an der Stelle des Cursors auf dem Display ausgegeben
            SPDisp.Write(produktName);
            //Da Preis "unten rechts" auf dem Display positioniert werden soll, muss die Länge vom Preis bestimmt werden um den Cursor passend zu setzen [{Py};{Px}H
            //+3 damit die Währung angezeigt werden kann
            SPDisp.Write($"\x1B[2;{20 - (Convert.ToString(Preis).Length + 3)}H");
            //Ausgabe Preis von Produkt
            SPDisp.Write(Convert.ToString(Preis));
            //Ausgabe der Währung
            SPDisp.Write(" EUR");
        }

        //EventHandler wenn serielle Daten vom Scanner kommen
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialDevice = sender as SerialPort;
            //Entgegennahme vom Scanner input
            string barcode = serialDevice.ReadLine();
            //Entfernen von mitgesendetem Zeilenumbruch
            barcode = barcode.Remove(barcode.Length - 1);

            //Deklaration der Variablen, die aus Datenbank abgefragt werden
            string bezeichnung;
            float preis;
            string hersteller;

            //SQL query string mit Barcode
            string sql = $"SELECT Bezeichnung, Preis, Hersteller FROM produkte WHERE EAN = {barcode}";
            //Initialisierung von MySqlCommand Instanz mit query und SQL Verbindung
            MySqlCommand command = new MySqlCommand(sql, connection);
            //SQL Verbindung wird neu eröffnet, falls sie geschlossen ist
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            
            //Query wird Ausgeführt
            MySqlDataReader reader = command.ExecuteReader();
            //Wenn reader eine Rückgabe hat, wird ausgeführt
            if(reader.Read())
            {
                //Zuweisung von Variablen aus SQL Query
                bezeichnung = reader.GetString("Bezeichnung");
                preis = reader.GetFloat("Preis");
                hersteller = reader.GetString("Hersteller");
                //Eingriff in Window Thread per Dispatcher und Lambda Ausdruck
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    //Erstellung von neuem Produkt Objekt und gleichzeitigem Hinzufügen zu bon_list
                    bon_list.Items.Add(new Produkt { Barcode = barcode, Hersteller = hersteller, Preis = preis, Name = bezeichnung });
                    //Preis des Produkts wird auch im Summen Feld angezeigt
                    bon_list_sum.Text = Convert.ToString(preis);
                    //Preis wird auf den Totalen Preis draufgerechnet
                    bon_list_total.Text = Convert.ToString(Math.Round(Convert.ToDouble(bon_list_total.Text) + preis, 3));
                }));
                //Anzeigen von gescanntem Produkt auf Display
                //Übergaben hierfür an die Funktion displayOnDisplay
                displayOnDisplay(bezeichnung, preis);
                Debug.WriteLine("Produkt gefunden");               
            }
            else 
            {
                //Fehlermeldung no product found (Mathis)
                Debug.WriteLine($"Produktfehler/Nicht in der Datenbank/{connection.State}");
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    outputWindow outwin = new outputWindow($"Produktfehler/Nicht in der Datenbank/\r\n SQL Con stat{connection.State}");
                    outwin.ShowDialog();
                }));                
            }
            //Reader wird geschlossen
            reader.Close();
            //Schließung fragwürdig
            connection.Close();            
        }

        //Verbindungsaufbau zur Datenbank
        public void Connect_to_SQL() 
        {
            //Verbindungs String für Datenbankverbindung mit eingesetzten Parametern
            string connectionString = $"Server={GLOBALS.SQL_IP};Database={GLOBALS.SQL_DB};Uid={GLOBALS.SQL_USER}";
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                //Bei gescheiterter Verbindung wird erneute Eingabe von Datenbankparametern gefordert
                Debug.WriteLine($"SQL Connection failed. SQL connection state: {connection.State}");
                
                //Eingriff in Window Thread per Dispatcher und Lambda Ausdruck 
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    //Erstellen von WindowSQLlogin
                    //Erneute Eingabe von SQL Parametern
                    WindowSQLlogin winsqllog = new WindowSQLlogin();
                    //Anzeige als Dialog
                    winsqllog.ShowDialog();
                }));
                //Selbstaufruf der Funktion zur erneuten Überprüfung der Loginparameter der SQL Datenbank
                Connect_to_SQL();
            }
            Debug.WriteLine("SQL connection successful");
        }

        //Funktionalität Manuelle EAN eingabe
        //Kein Fail Safe
        private void manualEANsubmit_Click(object sender, RoutedEventArgs e)
        {
            //barcode wird eine manuell eingegebene EAN zugewiesen
            string barcode = manualEAN.Text;

            //Deklaration von Variablen die aus der Datenbank abgefragt werden
            string bezeichnung;
            float preis;
            string hersteller;
            
            //SQL query string mit Barcode
            string sql = $"SELECT Bezeichnung, Preis, Hersteller FROM produkte WHERE EAN = {barcode}";
            //Initialisierung von MySqlCommand Instanz mit query und SQL verbindung
            MySqlCommand command = new MySqlCommand(sql, connection);
            //SQL verbindung wird neu eröffnet, falls sie geschlossen ist
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
                        //Preis vom Produkt wird auch im Summenfeld angezeigt
                        bon_list_sum.Text = Convert.ToString(preis);
                        //Preis wird auf den totalen Preis draufgerechnet
                        double gesamtBetrag = Convert.ToDouble(bon_list_total.Text);
                        gesamtBetrag += preis;
                        // gesamtBetrag wird auf 2 Nachkommastellen gerundet als String in bon_list_total.Text reingeschrieben
                        bon_list_total.Text = gesamtBetrag.ToString("F2");
                    }));
                    //Anzeigen von gescanntem Produkt Auf Display
                    //Übergaben hierfür an Funktion displayOnDisplay
                    displayOnDisplay(bezeichnung, preis);
                    Debug.WriteLine("Produkt gefunden");
                }
                else
                {
                    //Fehlermeldung: Produkt nicht gefunden
                    Debug.WriteLine($"Produktfehler/Nicht in der Datenbank/{connection.State}");
                    outputWindow outwin = new outputWindow($"Produktfehler/Nicht in der Datenbank/\r\n SQL Verbindungsstatus: {connection.State}");
                    outwin.ShowDialog();
                }
                //Reader wird geschlossen
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
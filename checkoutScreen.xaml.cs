﻿using Org.BouncyCastle.Math;
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
using System.Windows.Controls.Primitives;
using System.IO.Ports;

namespace _202223_bbs_projekt_kasse
{
    /// <summary>
    /// Interaction logic for checkoutScreen.xaml
    /// Bestimmung von Zahlungsmethode und ggf Eingabe von gegebenem Geld zur späteren Bestimmung des Rückgeldes 
    /// </summary>
    public partial class checkoutScreen : Window
    {
        //Annahme vom Serial Port
        public SerialPort SPDisp = GLOBALS.SPDisp;

        public checkoutScreen()
        {
            InitializeComponent();
            //Erstellen und Starten vom Display-Prozess
            Thread Display = new Thread(displayOutput);
            Display.Start();
            // Total wird auf 2 Nachkommastellen gerundet als String in bon_list_total geschrieben
            bon_list_total.Text = GLOBALS.Total.ToString("F2");
            //Übernahme der Produkte des Bons
            foreach (var item in GLOBALS.currentBon)
            {
                bon_list.Items.Add(item);
            }
        }

        public void displayOutput()
        {
            //Konfiguration COM Port
            SPDisp = new SerialPort(GLOBALS.COM_PORT_DISP);
            SPDisp.BaudRate = 9600;
            SPDisp.Parity = Parity.Odd;
            SPDisp.StopBits = StopBits.One;
            SPDisp.DataBits = 8;
            SPDisp.Handshake = Handshake.None;
            SPDisp.RtsEnable = true;
            SPDisp.DtrEnable = true;

            //Versuchter Verbindungsaufbau
            try
            {
                SPDisp.Open();
            }
            catch (Exception)
            {
                //Fehlermeldung: Falscher COM Port!
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    comPortCorrection comcor = new comPortCorrection(false);
                    comcor.ShowDialog();
                }));
                displayOutput();
            }
        }

        //Nachfolgend bis numpad_but_double0_Click
        //Funktionalität des Tastenfelds
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

        private void numpad_but_decimal_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += ".";
        }

        private void numpad_but_0_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "0";
        }

        private void numpad_but_double0_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content += "00";
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            SPDisp.Close();
            this.Close();
        }

        //Funktionalität "C" Knopf
        //Löscht die letzte Stelle von numpad_output1
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            //Überprüfung auf Inhalt 
               
            if(numpad_output1.Content != null && numpad_output1.Content.ToString().Length != 0)
            {
                numpad_output1.Content = numpad_output1.Content.ToString().Remove(numpad_output1.Content.ToString().Length - 1);
            }

            
        }

        //Funktionalität "Bar" Knopf
        private void cash_Click(object sender, RoutedEventArgs e)
        {
            //Überprüfen, ob eine gegebene Summe eingegeben wurde, wenn nicht, dann Annahme, dass passend bezahlt wurde 
            if (numpad_output1.Content == null || Convert.ToInt32(numpad_output1.Content.ToString()) == 0)
            {
                //Erstellung paymentSuccessful
                paymentSuccessful paymsuc = new paymentSuccessful();
                //paymentSuccessful wird angezeigt
                paymsuc.Show();
                //checkoutScreen wird geschlossen
                this.Close();
            }
            else
            {
                //changeScreen wird mit zu übergebenden Werten erstellt
                changeScreen chascr = new changeScreen(Convert.ToDouble(bon_list_total.Text), Convert.ToDouble(numpad_output1.Content));
                //changeScreen wird als Dialog angezeigt
                chascr.ShowDialog();
                //checkoutScreen wird geschlossen
                this.Close();
            }
            //Kundendisplay wird geleert
            SPDisp.Write("\x1B[2J");
            //Serielle Verbindung wird geschlossen
            SPDisp.Close();
        }
    }
}

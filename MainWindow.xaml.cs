using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace _202223_bbs_projekt_kasse
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>

    public delegate void bon_l(string s);
    public partial class MainWindow : Window
    {
        public int op_mode = 0;
        SerialPort serialPort = null;

        public MainWindow()
        {
            InitializeComponent();
            

            Thread scanning = new Thread(MyCommPort);
            scanning.Start();

            bon_l del = (s) =>
            {
                bon_list.Items.Add(s);
            };
        }


        private void numpad_but_div_Click(object sender, RoutedEventArgs e)
        {
            op_mode = 1;
        }
        private void numpad_but_mul_Click(object sender, RoutedEventArgs e)
        {
            op_mode = 2;
        }
        private void numpad_but_add_Click(object sender, RoutedEventArgs e)
        {
            op_mode = 3;
        }
        private void numpad_but_sub_Click(object sender, RoutedEventArgs e)
        {
            op_mode = 4;
        }
        private void numpad_but_1_Click(object sender, RoutedEventArgs e)
        {
            if (op_mode != 0)
            {
                numpad_output2.Content += "1";
            }
            else
            {
                numpad_output1.Content += "1";
            }
            
        }

        private void numpad_but_2_Click(object sender, RoutedEventArgs e)
        {
            if (op_mode != 0)
            {
                numpad_output2.Content += "2";
            }
            else
            {
                numpad_output1.Content += "2";
            }
        }

        private void numpad_but_3_Click(object sender, RoutedEventArgs e)
        {
            if (op_mode != 0)
            {
                numpad_output2.Content += "3";
            }
            else
            {
                numpad_output1.Content += "3";
            }
        }

        private void numpad_but_4_Click(object sender, RoutedEventArgs e)
        {
            if (op_mode != 0)
            {
                numpad_output2.Content += "4";
            }
            else
            {
                numpad_output1.Content += "4";
            }
        }

        private void numpad_but_5_Click(object sender, RoutedEventArgs e)
        {
            if (op_mode != 0)
            {
                numpad_output2.Content += "5";
            }
            else
            {
                numpad_output1.Content += "5";
            }
        }

        private void numpad_but_6_Click(object sender, RoutedEventArgs e)
        {
            if (op_mode != 0)
            {
                numpad_output2.Content += "6";
            }
            else
            {
                numpad_output1.Content += "6";
            }
        }

        private void numpad_but_7_Click(object sender, RoutedEventArgs e)
        {
            if (op_mode != 0)
            {
                numpad_output2.Content += "7";
            }
            else
            {
                numpad_output1.Content += "7";
            }
        }

        private void numpad_but_8_Click(object sender, RoutedEventArgs e)
        {
            if (op_mode != 0)
            {
                numpad_output2.Content += "8";
            }
            else
            {
                numpad_output1.Content += "8";
            }
        }

        private void numpad_but_9_Click(object sender, RoutedEventArgs e)
        {
            if (op_mode != 0)
            {
                numpad_output2.Content += "9";
            }
            else
            {
                numpad_output1.Content += "9";
            }
        }

        private void numpad_but_0_Click(object sender, RoutedEventArgs e)
        {
            if (op_mode != 0)
            {
                numpad_output2.Content += "0";
            }
            else
            {
                numpad_output1.Content += "0";
            }
        }

        private void numpad_but_ent_Click(object sender, RoutedEventArgs e)
        {
            switch (op_mode)
            {
                case 0:
                    {
                        //add to list
                        bon_list.Items.Add(numpad_output1.Content);
                        numpad_output1.Content = null;
                        break;
                    }
                case 1:
                    {
                        //DIV
                        numpad_output1.Content = System.Convert.ToInt32(numpad_output1.Content) / System.Convert.ToInt32(numpad_output2.Content);
                        numpad_output2.Content = null;
                        break;
                    }
                    
                default:
                    break;
            }
            op_mode = 0;
        }

        private void numpad_but_clear_Click(object sender, RoutedEventArgs e)
        {
            numpad_output1.Content = null;
        }
        public void MyCommPort()
        {
            serialPort = new SerialPort("COM4");
            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.Odd;
            serialPort.StopBits = StopBits.One;
            serialPort.DataBits = 8;
            serialPort.Handshake = Handshake.None;
            serialPort.RtsEnable = true;
            serialPort.DtrEnable = true;

            serialPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);
            serialPort.Open();
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialDevice = sender as SerialPort;
            var buffer = new byte[serialDevice.BytesToRead];
            serialDevice.Read(buffer, 0, buffer.Length);

            Application.Current.Dispatcher.Invoke(new Action(() => {
                string aggr = null;
                foreach (var item in buffer)
                {
                    aggr += Convert.ToChar(item);                
                }
                bon_list.Items.Add(aggr);
            }));
        }
    }
}

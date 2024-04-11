using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Wpf.Ui.Controls;
using static System.Net.Mime.MediaTypeNames;
using Button = Wpf.Ui.Controls.Button;

namespace PassWordManager
{
    public class PassWordInfo
    {
        public String OriginPW
        {
             get;
             set;
        }
        public int ExtraLengthLevel
        {
            get;
            set;
        }

        public String Key
        {
            get;
            set;
        }

        public int CheckInt
        {
            get;
            set;
        }

        public int OriginLength
        {
            get;
            set;
        }

        public String EnAlgorithmArrary
        {
            get;
            set;
        }
    }

    public class PassWordInfoSimple
    {
        public String OriginPW
        {
            get;
            set;
        }
        public int ExtraLengthLevel
        {
            get;
            set;
        }

        public String Key
        {
            get;
            set;
        }

        public String EnAlgorithmArrary
        {
            get;
            set;
        }
    }
    
    public delegate int EnAlgorithms (int key, int word, int all, int i, int length);

    /// <summary>
    /// PassWordPage.xaml 的交互逻辑
    /// </summary>
    public partial class PassWordPage : Page, IHostedService
    {
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            InitializeComponent();
            
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            
        }
        public static void JsonLogWritePDE (string path,string json)
        {
            using ( StreamWriter file = File.AppendText(path) )
            {
                file.WriteLine(json);
            }
        }
        
        private static String defautOriginPW;
        public class ButtonPair
        {
            public ButtonPair(Button button)
            {
                Button = button;
                State = false;
            }
            public Button Button;
            public bool State;
        }

        public ButtonPair[] ButtonPairs;

        public delegate int EnAlgorithm(int key, int word, int all, int i, int length);
        public EnAlgorithm[] EnAlgorithms;
        private String path;
        
        public Button[] MethodButtons;
        
        public PassWordPage()
        {
            InitializeComponent();
            defautOriginPW = "password";
            
            ButtonPairs = new ButtonPair[5];
            ButtonPairs[0] = new ButtonPair(EnA1);
            ButtonPairs[1] = new ButtonPair(EnA2);
            ButtonPairs[2] = new ButtonPair(EnA3);
            ButtonPairs[3] = new ButtonPair(EnA4);
            ButtonPairs[4] = new ButtonPair(EnA5);

            ButtonPairs[0].State = true;
            ButtonPairs[1].State = true;
            
            StateColor(EnA1);
            StateColor(EnA2);

            EnAlgorithms = new EnAlgorithm[5];

            EnAlgorithms[0] = Encrypt.EnAlgorithm1;
            EnAlgorithms[1] = Encrypt.EnAlgorithm2;
            EnAlgorithms[2] = Encrypt.EnAlgorithm3;
            EnAlgorithms[3] = Encrypt.EnAlgorithm4;
            EnAlgorithms[4] = Encrypt.EnAlgorithm5;

            path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(path);
            path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/PDELog.Json");
            
            LogState = logState.None;
            LogButtonActvie(LogB1, true);
            
        }

        public enum logState
        {
            Simple,
            Detail,
            None
        } 

        public logState LogState;
        public void ContentSwap(Button button)
        {
            ButtonPair BPa;
            ButtonPair BPb;
            int position = int.Parse(button.Tag.ToString());
            BPa = ButtonPairs[position];
            if (position == 0)
            {
                BPb = ButtonPairs[position];
            }
            else
            {
                BPb = ButtonPairs[position-1];
            }

            var content = BPa.Button.Content.ToString();
            BPa.Button.Content = BPb.Button.Content;
            BPb.Button.Content = content;
            
            var state = BPa.State;
            BPa.State = BPb.State;
            BPb.State = state;
            
            StateColor(BPa.Button);
            StateColor(BPb.Button);
            

        }

        private void StateColor(Button button)
        {
            var postion = int.Parse(button.Tag.ToString());
            var ButtonPair = ButtonPairs[postion];
            if (ButtonPair.State)
            {
                button.Background = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
                button.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                button.Background = new SolidColorBrush(Colors.White);
                button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        
        private void MethodChoosen(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int position = int.Parse(button.Tag.ToString());
            var temp = ButtonPairs[position];
            if (temp.State == true)
            {
                temp.State = false;
            }
            else
            {
                temp.State = true;
            }
            StateColor(button);

        }

        private void MethodMove(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Button;
            ContentSwap(button);
        }

        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            String okey,oword,res= "",key = KeyBox.Text,word = WordBox.Text;
            if (key.Length == 0)
            {
                System.Windows.MessageBox.Show("请检查密钥是否为空","加密失败", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information); 
                return;
            }
            if (word.Length == 0)
            {
                word += defautOriginPW;
            }
            double temps = Math.Sqrt(key.Length);
            int ts=0,all = 0,tt = 0,length = 8 + 4 * (int)Math.Sqrt(key.Length * 5 - 11) - (int)Math.Pow(temps, 3.0);
            length = Encrypt.LengthControl(length)+GetExtraLength();
            okey = key; oword = word;
            while ( word.Length < length + 1 )
            {
                word += word;
            }
            while ( key.Length < length + 1 )
            {
                key += key;
            }

            foreach (char temp in key)
            {
                all += temp;
            }

            foreach ( char temp in word )
            {
                all += temp;
            }

            int[] Methods = GetEncryptLists();
            
            for(int i = 0;i < length; i++ )
            {
                ts = 0;
                ts = EnAlgorithms[Methods[tt]-1](key[i], word[i], all, i,length);
                tt++;
                if ( tt == Methods.Length )
                    tt = 0;
                ts = Encrypt.CharControl(ts);
                res += (char)ts;
            }
            
            if (LogState == logState.Detail)
            {
                all = 0;
                foreach (var chart in res)
                {
                    all += chart;
                }
                String EnAArrary = "";
                foreach ( int i in Methods )
                {
                    EnAArrary += i.ToString();
                }
                var Info = new PassWordInfo()
                {
                    ExtraLengthLevel = (int)ExtraLength.Value,
                    Key = okey,
                    OriginPW = oword,
                    OriginLength = length,
                    CheckInt = all,
                    EnAlgorithmArrary = EnAArrary
                };
                String json = JsonConvert.SerializeObject(Info, Formatting.Indented);
                JsonLogWritePDE(path,json);
            }

            if (LogState == logState.Simple)
            {
                String EnAArrary = "";
                foreach ( int i in Methods)
                {
                    EnAArrary += i.ToString();
                }
                var Info = new PassWordInfoSimple()
                {
                    OriginPW = oword,
                    EnAlgorithmArrary = EnAArrary,
                    ExtraLengthLevel = (int)(ExtraLength.Value),
                    Key = okey
                };
                String json = JsonConvert.SerializeObject(Info, Formatting.Indented);
                JsonLogWritePDE(path,json);
            }
            ResultBox.Text = res;

        }

        private void LogBClick(object sender, RoutedEventArgs e)
        {
            LogButtonActvie(LogB1, false);
            LogButtonActvie(LogB2, false);
            LogButtonActvie(LogB3, false);
            var button = sender as Button;
            switch ( int.Parse(button.Tag.ToString()) )
            {
                case 0:
                    LogState = logState.None;
                    break;
                case 1:
                    LogState = logState.Simple;
                    break;
                case 2:
                    LogState = logState.Detail;
                    break;
            }
            LogButtonActvie(button, true);
        }

        public int GetExtraLength ()
        {
           int level =  (int)ExtraLength.Value;
            switch (level)
            {
                case 0:
                    return 0;
                case 1:
                    return 4;
                case 2:
                    return 8;
                case 3:
                    return 12;
               case 4:
                    return 16;
               default:
                    return 0;
            }
        }

        public int GetMethodInfo(Button button)
        {
            char temp = button.Content.ToString()[2];
            return int.Parse(""+temp);
        }
        
        public int[] GetEncryptLists()
        {
            int i = 0;
            List<int> Lists = new List<int>();
            foreach (var buttonPair in ButtonPairs)
            {
                if (buttonPair.State == true)
                {
                    i++;
                    Lists.Add(GetMethodInfo(buttonPair.Button));
                }
            }
            return Lists.ToArray();
        }

        private void LogBClicck(object sender, RoutedEventArgs e)
        {
            
        }

        private void LogButtonActvie(Button button,bool state)
        {
            if (state)
            {
                button.Background = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
                button.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
               button.Background = new SolidColorBrush(Colors.White);
               button.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void ExtraLength_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            var url = button.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}


    public static class Encrypt
    {
        public static char CharControl(int a)
        {
            while ( 33 > a || a > 126 )
            {
                if ( a > 126 )
                {
                    a -= 126;
                }
                if ( a < 33 )
                {
                    a += 33;
                }
            }
            return (char)a;
        }

        public static int LengthControl(int a)
        {
            
            while ( 8 > a || a > 20 )
            {
                if ( a > 17 )
                {
                    a -= 17;
                }
                if ( a < 8 )
                {
                    a += 8;
                }
            }
            return a;
        }

        public static int EnAlgorithm1(int key, int word, int all, int i, int length)
        {
             return ( length * i * key + 4 * all + 3 * word + (int)Math.Pow(key, 3) - 4 * length );
        }

        public static int EnAlgorithm2(int key, int word, int all, int i, int length)
        {
            return ( all * (int)Math.Sqrt(key * word + 8 * key + 5 * word) - word * all ) + length * i;
        }

        public static int EnAlgorithm3(int key, int word, int all, int i, int length)
        {
             return EnAlgorithm1(key, word, all, i, length)+EnAlgorithm2(key, word, all, i, length);
         }

        public static int EnAlgorithm4(int key, int word, int all, int i, int length)
        {
            return EnAlgorithm1(key, word, all, i, length) - EnAlgorithm2(key, word, all, i, length);
        }

        public static int EnAlgorithm5(int key, int word, int all, int i, int length)
        {
            return EnAlgorithm1(key, word, all, i, length) * EnAlgorithm2(key, word, all, i, length);
        }

    }

        


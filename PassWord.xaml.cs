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
using Microsoft.Extensions.Hosting;

namespace PassWordManager
{
    /// <summary>
    /// PassWord.xaml 的交互逻辑
    /// </summary>
    public partial class PassWord : Window, IHostedService
    {
        //public App App;

        public PassWord()
        {
            InitializeComponent();
            //App = app;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            InitializeComponent();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //App.Over();
            Application.Current.Shutdown();
        }
    }
}

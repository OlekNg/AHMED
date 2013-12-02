using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main.View
{
    /// <summary>
    /// Interaction logic for StatusWindow.xaml
    /// </summary>
    public partial class StatusWindow : Window
    {
        private Action _stopAction;

        public StatusWindow(Action StopAction)
        {
            InitializeComponent();
            _stopAction = StopAction;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _stopAction();
        }
    }
}

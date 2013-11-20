using PropertyChanged;
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

namespace BuildingEditor
{
    /// <summary>
    /// Interaction logic for RangeWindow.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class RangeWindow : Window
    {
        public RangeWindow()
        {
            InitializeComponent();
        }

        public void GetValues(out int a, out int b)
        {
            a = 3;
            b = 4;
        }
    }
}

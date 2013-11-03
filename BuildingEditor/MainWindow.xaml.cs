using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFTest.Logic;

namespace WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Building _building;
        private Random _randomizer;

        public MainWindow()
        {
            InitializeComponent();
            _building = new Building();

            lst.ItemsSource = _building.Data;

            ObservableCollection<Tool> toolbox = new ObservableCollection<Tool>();
            toolbox.Add(new FloorTool(_building));
            toolbox.Add(new WallTool(_building));

            Toolbox.ItemsSource = toolbox;

            _randomizer = new Random();
        }

        private void uxWorkspaceGrid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
        }

        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Tool selectedTool = (Tool)Toolbox.SelectedItem;

            if (selectedTool != null)
                selectedTool.MouseDown(sender, e);
        }
    }
}

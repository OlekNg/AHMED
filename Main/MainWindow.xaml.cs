using BuildingEditor.Logic;
using Common.DataModel.Enums;
using Simulation;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Building _building;

        public MainWindow()
        {
            InitializeComponent();
            _building = new Building();

            Common.DataModel.Building building = new Common.DataModel.Building();
            building.Load("officialtest.xml");
            Building viewModel = new Building(building);


            _building.Floors = viewModel.Floors;
            _building.Stairs = viewModel.Stairs;
            _building.CurrentFloor = _building.Floors[0];

            uxWorkspaceViewbox.DataContext = _building;
            uxFloors.DataContext = _building;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".xml";
            dlg.Filter = "Building definition file|*.xml";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                Common.DataModel.Building building = new Common.DataModel.Building();
                building.Load(filename);
                Building viewModel = new Building(building);


                _building.Floors = viewModel.Floors;
                _building.Stairs = viewModel.Stairs;
                _building.CurrentFloor = _building.Floors[0];
            }
        }

        private void ViewMode_Clicked(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            string mode = element.Tag as string;

            if (mode != null)
                _building.ViewMode = mode;
        }

        private void ProcessRun_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            List<Direction> fenotype = new List<Direction>() { Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.DOWN,
                                                               Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.UP };
            _building.SetFenotype(fenotype);
            _building.DrawSolution();

            MapBuilder mapBuilder = new MapBuilder(_building.ToDataModel());
            Simulator sim = new Simulator();

            sim.MaximumTicks = 50;
            sim.SetupSimulator(mapBuilder.BuildBuildingMap(), mapBuilder.BuildPeopleMap());
            var result = sim.Simulate(_building.GetSimulatorFenotype());

            foreach (var group in result)
            {
                Console.WriteLine("Escaped group: quantity {0}, ticks {1}", group.Quantity, group.Ticks);
            }
        }

        private void Test2_Click(object sender, RoutedEventArgs e)
        {
            List<Direction> fenotype = new List<Direction>() { Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.DOWN,
                                                               Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.DOWN,
                                                               Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.DOWN,
                                                               Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.DOWN,
                                                               Direction.RIGHT, Direction.RIGHT, Direction.DOWN, Direction.LEFT, Direction.LEFT };
            _building.SetFenotype(fenotype);
            _building.DrawSolution();
        }

        private void ToolsEditor_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(typeof(BuildingEditor.App).Assembly.Location);
        }

        
    }
}

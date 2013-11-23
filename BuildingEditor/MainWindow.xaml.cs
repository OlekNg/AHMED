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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BuildingEditor.Logic;
using BuildingEditor.Tools.Logic;
using System.Xml.Serialization;
using System.IO;
using Common.DataModel.Enums;

namespace BuildingEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ISegmentEventHandler
    {
        private Building _building;
        private Tool _currentTool;
        private Random _randomizer;

        public MainWindow()
        {
            InitializeComponent();
            _building = new Building(5, 5);

            uxWorkspaceViewbox.DataContext = _building;

            uxFloors.DataContext = _building;
            _building.CurrentFloor = _building.Floors[0];
            
            uxStairs.ItemsSource = _building.Stairs;

            ObservableCollection<Tool> toolbox = new ObservableCollection<Tool>();
            toolbox.Add(new DragTool(uxWorkspaceViewbox, uxModePanel));
            toolbox.Add(new FloorTool(_building));
            toolbox.Add(new SideElementTool(_building, SideElementType.WALL, "Wall"));
            toolbox.Add(new SideElementTool(_building, SideElementType.DOOR, "Door") { Capacity = 5 });
            toolbox.Add(new PeopleTool(_building));
            toolbox.Add(new DeleteTool(_building));
            toolbox.Add(new StairsTool(_building));

            uxToolbox.ItemsSource = toolbox;
            uxToolbox.SelectedIndex = 0;

            _randomizer = new Random();

            SegmentEventHandler.Register(this);
        }

        private void Workspace_MouseLeave(object sender, MouseEventArgs e)
        {
            // Clear preview when out of workspace.
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;
            if (selectedTool != null)
                selectedTool.ClearPreview();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Segment events handlers.
        public void Segment_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;

            if (selectedTool != null)
                selectedTool.MouseDown(sender, e);
        }

        public void Segment_MouseMove(object sender, MouseEventArgs e)
        {
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;

            if (selectedTool != null)
                selectedTool.MouseMove(sender, e);
        }

        public void Segment_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;

            if (selectedTool != null)
                selectedTool.MouseUp(sender, e);
        }

        public void Segment_MouseEnter(object sender, MouseEventArgs e)
        {
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;

            if (selectedTool != null)
                selectedTool.MouseEnter(sender, e);
        }

        public void Segment_MouseLeave(object sender, MouseEventArgs e)
        {
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;

            if (selectedTool != null)
                selectedTool.MouseLeave(sender, e);
        }

        /// <summary>
        /// Performs zoom in/out of the building.
        /// </summary>
        private void Workspace_MouseWHeel(object sender, MouseWheelEventArgs e)
        {
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;

            if (selectedTool != null)
                selectedTool.MouseWheel(sender, e);
        }
        #endregion

        /// <summary>
        /// Adds new floor to the building.
        /// </summary>
        private void AddFloor_Click(object sender, RoutedEventArgs e)
        {
            int rows, cols;
            if (!Int32.TryParse(uxCols.Text, out cols)) return;
            if (!Int32.TryParse(uxRows.Text, out rows)) return;

            _building.AddFloor(rows, cols);
        }

        /// <summary>
        /// Expands current floor.
        /// </summary>
        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            _building.CurrentFloor.Expand((Side)b.Tag);
        }

        /// <summary>
        /// Prevents focus on capacity texbox.
        /// </summary>
        private void Workspace_MouseEnter(object sender, MouseEventArgs e)
        {
            uxWorkspaceCanvas.Focus();
        }

        private void Toolbox_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (_currentTool != null)
                _currentTool.CancelAction();

            _currentTool = (Tool)uxToolbox.SelectedItem;
            _building.ViewMode = _currentTool.Name;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".xml";
            dlg.Filter = "Building definition file|*.xml";

            // Display SaveFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                string filename = dlg.FileName;
                Console.WriteLine(filename);
                Common.DataModel.Building building = _building.ToDataModel();
                building.Save(filename);
            }
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
                uxStairs.ItemsSource = _building.Stairs;
            }
        }
    }
}

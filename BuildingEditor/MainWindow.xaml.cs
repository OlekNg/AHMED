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

namespace BuildingEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Building _building;
        private Tool _currentTool;
        private Random _randomizer;

        public MainWindow()
        {
            InitializeComponent();
            _building = new Building();

            uxFloors.DataContext = _building;

            ObservableCollection<StairsPair> stairs = new ObservableCollection<StairsPair>();
            uxStairs.ItemsSource = stairs;

            ObservableCollection<Tool> toolbox = new ObservableCollection<Tool>();
            toolbox.Add(new DragTool(uxWorkspaceViewbox, uxModePanel));
            toolbox.Add(new FloorTool(_building));
            toolbox.Add(new SideElementTool(_building, SideElementType.WALL, "Wall"));
            toolbox.Add(new SideElementTool(_building, SideElementType.DOOR, "Door") { Capacity = 5 });
            toolbox.Add(new PeopleTool(_building));
            toolbox.Add(new DeleteTool(_building));
            toolbox.Add(new StairsTool(_building, stairs));

            uxToolbox.ItemsSource = toolbox;
            uxToolbox.SelectedIndex = 0;

            _randomizer = new Random();
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
        private void Segment_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;

            if (selectedTool != null)
                selectedTool.MouseDown(sender, e);
        }

        private void Segment_MouseMove(object sender, MouseEventArgs e)
        {
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;

            if (selectedTool != null)
                selectedTool.MouseMove(sender, e);
        }

        private void Segment_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;

            if (selectedTool != null)
                selectedTool.MouseUp(sender, e);
        }

        private void Segment_MouseEnter(object sender, MouseEventArgs e)
        {
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;

            if (selectedTool != null)
                selectedTool.MouseEnter(sender, e);
        }

        private void Segment_MouseLeave(object sender, MouseEventArgs e)
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
        }
    }
}

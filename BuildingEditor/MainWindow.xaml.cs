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

            uxFloors.DataContext = _building;

            ObservableCollection<Tool> toolbox = new ObservableCollection<Tool>();
            toolbox.Add(new DragTool(uxWorkspace, uxModePanel));
            toolbox.Add(new FloorTool(_building));
            toolbox.Add(new SideElementTool(_building, SideElementType.WALL, "Wall"));
            toolbox.Add(new SideElementTool(_building, SideElementType.DOOR, "Door"));

            uxToolbox.ItemsSource = toolbox;
            uxToolbox.SelectedIndex = 0;

            _randomizer = new Random();
        }

        private void Workspace_MouseLeave(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Workspace leave");
            // Cancel any action that is being performed by selected tool.
            Tool selectedTool = (Tool)uxToolbox.SelectedItem;
            if (selectedTool != null)
                selectedTool.CancelAction();
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
        #endregion

        /// <summary>
        /// Performs zoom in/out of the building.
        /// </summary>
        private void Workspace_MouseWHeel(object sender, MouseWheelEventArgs e)
        {
            TransformGroup group = (TransformGroup)uxWorkspace.RenderTransform;

            var st = group.Children.OfType<ScaleTransform>().First();
            double deltaScale = e.Delta > 0 ? 0.2 : -0.2;
            st.ScaleX += deltaScale;
            st.ScaleY += deltaScale;
            Console.WriteLine("Scale {0}", st.ScaleY);
        }

        private void AddFloor_Click(object sender, RoutedEventArgs e)
        {
            int rows, cols;
            if (!Int32.TryParse(uxRows.Text, out rows)) return;
            if (!Int32.TryParse(uxColumns.Text, out cols)) return;

            if (rows < 1 || cols < 1) return;

            _building.AddFloor(rows, cols);
        }

        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            _building.CurrentFloor.Expand((Side)b.Tag);
        }
    }
}

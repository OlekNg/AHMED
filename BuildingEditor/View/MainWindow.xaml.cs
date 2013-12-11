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
using BuildingEditor.ViewModel;
using BuildingEditor.ViewModel.Tools;
using System.Xml.Serialization;
using System.IO;
using Common.DataModel.Enums;

namespace BuildingEditor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Editor _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new Editor(uxWorkspaceViewbox, this);
            _viewModel.NewBuilding();

            DataContext = _viewModel;
        }

        public MainWindow(string file)
            : this()
        {
            _viewModel.LoadBuilding(file);
        }

        

        #region Workspace event handlers.
        private void Workspace_MouseLeave(object sender, MouseEventArgs e)
        {
            _viewModel.Workspace_MouseLeave(sender, e);
        }

        /// <summary>
        /// Prevents focus on capacity texbox.
        /// </summary>
        private void Workspace_MouseEnter(object sender, MouseEventArgs e)
        {
            uxWorkspaceCanvas.Focus();
        }
        #endregion

        #region Actions
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
                _viewModel.LoadBuilding(dlg.FileName);
            }
        }

        private void RemoveFloor_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            _viewModel.RemoveFloor((int)b.Tag);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CurrentFile == "")
                SaveAs_Click(sender, e);
            else
            {
                _viewModel.SaveBuilding();
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
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
                _viewModel.SaveBuilding(dlg.FileName);
            }
        }

        private void Toolbox_Selected(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.Toolbox_Selected(sender, e);
        }
        #endregion
    }
}

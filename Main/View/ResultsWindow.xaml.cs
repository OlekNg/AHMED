using Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main.View
{
    /// <summary>
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        private ResultsViewer viewModel;
        private List<string> showProperties = new List<string>() { "ShowAverageFitness", "ShowBestChromosome", "ShowCpuUsage" };
        public ResultsWindow(Calculator calculatorModel)
        {
            InitializeComponent();
            viewModel = new ResultsViewer(calculatorModel);
            DataContext = viewModel;
            viewModel.PropertyChanged += OnModelChanged;
        }

        private void OnModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            BestChromosomePlot.InvalidatePlot();
            AvgFitnessPlot.InvalidatePlot();
            IterationTimePlot.InvalidatePlot();
            CpuUsagePlot.InvalidatePlot();
        }

        private void SelectedResultSets_Changed(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as System.Windows.Controls.ListView;
            var selected = list.SelectedItems;

            if (selected.Count > 2)
            {
                var diff = selected.Count - 2;
                for (var i = 0; i < diff; i++)
                    selected.RemoveAt(0);
            }

            if (selected.Count > 0)
                viewModel.SelectedResultSet = selected[0] as ResultSet;
            else
                viewModel.SelectedResultSet = null;

            if (selected.Count > 1)
                viewModel.SelectedResultSet2 = selected[1] as ResultSet;
            else
                viewModel.SelectedResultSet2 = null;
        }
    }
}

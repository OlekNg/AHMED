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
    }
}

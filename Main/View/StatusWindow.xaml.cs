using BuildingEditor.ViewModel;
using Main.ViewModel;
using OxyPlot.Series;
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
        public StatusWindow(Status statusModel)
        {
            InitializeComponent();
            DataContext = statusModel;

            statusModel.PropertyChanged += Test;
        }

        private void Test(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StatisticsChanged")
            {
                Dispatcher.BeginInvoke(new Action(() => {
                    AvgFitnessPlot.InvalidatePlot();
                    BestChromosomePlot.InvalidatePlot();
                    CpuUsagePlot.InvalidatePlot();
                }));
            }
        }
    }
}

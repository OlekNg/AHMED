using BuildingEditor.ViewModel;
using Genetics;
using OxyPlot;
using OxyPlot.Series;
using PropertyChanged;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

namespace Main.ViewModel
{
    /// <summary>
    /// View model for GeneticStatusControl.
    /// </summary>
    [ImplementPropertyChanged]
    public class Status : INotifyPropertyChanged
    {
        public Status(GeneticAlgorithm ga)
        {
            ga.ReportStatus += OnReportStatus;
            StopCommand = new SimpleCommand(x => ga.Stop());
            AvgFitness = new BlockingCollection<DataPoint>();
            BestChromosome = new BlockingCollection<DataPoint>();
            Selection = new BlockingCollection<DataPoint>();
            Crossover = new BlockingCollection<DataPoint>();
            Mutation = new BlockingCollection<DataPoint>();
            Repair = new BlockingCollection<DataPoint>();
            Transform = new BlockingCollection<DataPoint>();
            Evaluation = new BlockingCollection<DataPoint>();
        }

        public ICommand StopCommand { get; set; }
        public BlockingCollection<DataPoint> AvgFitness { get; set; }
        public BlockingCollection<DataPoint> BestChromosome { get; set; }
        public BlockingCollection<DataPoint> Selection { get; set; }
        public BlockingCollection<DataPoint> Mutation { get; set; }
        public BlockingCollection<DataPoint> Evaluation { get; set; }
        public BlockingCollection<DataPoint> Repair { get; set; }
        public BlockingCollection<DataPoint> Transform { get; set; }
        public BlockingCollection<DataPoint> Crossover { get; set; }

        #region Properties
        public double SelectionOverhead { get; set; }
        public double CrossoverOverhead { get; set; }
        public double MutationOverhead { get; set; }
        public double RepairOverhead { get; set; }
        public double TransformOverhead { get; set; }
        public double EvaluationOverhead { get; set; }

        public int MaxIterations { get; set; }
        public int CurrentIteration { get; set; }
        public int PercentCompleted { get; set; }
        public string ProgressInfo { get; set; }
        public double BestChromosomeValue { get; set; }
        #endregion

        private void OnReportStatus(GeneticAlgorithmStatus status)
        {
            var pointHelper = new PointHelper(status.IterationNumber);
            AvgFitness.Add(pointHelper.Create(status.CurrentPopulation.AvgFitness));
            BestChromosome.Add(pointHelper.Create(status.BestChromosome.Value));
            Selection.Add(pointHelper.Create(status.SelectionOverhead));
            Crossover.Add(pointHelper.Create(status.CrossoverOverhead));
            Mutation.Add(pointHelper.Create(status.MutationOverhead));
            Repair.Add(pointHelper.Create(status.RepairOverhead));
            Transform.Add(pointHelper.Create(status.TransformOverhead));
            Evaluation.Add(pointHelper.Create(status.EvaluationOverhead));

            SelectionOverhead = status.SelectionOverhead;
            CrossoverOverhead = status.CrossoverOverhead;
            MutationOverhead = status.MutationOverhead;
            RepairOverhead = status.RepairOverhead;
            TransformOverhead = status.TransformOverhead;
            EvaluationOverhead = status.EvaluationOverhead;

            MaxIterations = status.MaxIterations;
            CurrentIteration = status.IterationNumber;
            PercentCompleted = (CurrentIteration * 100) / MaxIterations;
            BestChromosomeValue = status.BestChromosome.Value;

            ProgressInfo = String.Format("Iteration {0} of {1}", CurrentIteration, MaxIterations);

            OnPropertyChanged("StatisticsChanged");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private class PointHelper
        {
            private double _x;

            public PointHelper(double x)
            {
                _x = x;
            }

            public DataPoint Create(double y)
            {
                return new DataPoint(_x, y);
            }
        }
    }
}

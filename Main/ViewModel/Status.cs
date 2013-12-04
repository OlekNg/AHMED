using BuildingEditor.ViewModel;
using Genetics;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Main.ViewModel
{
    /// <summary>
    /// View model for GeneticStatusControl.
    /// </summary>
    [ImplementPropertyChanged]
    public class Status
    {
        public Status(GeneticAlgorithm ga)
        {
            ga.ReportStatus += OnReportStatus;
            StopCommand = new SimpleCommand(x => ga.Stop());
        }

        public ICommand StopCommand { get; set; }

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
        }
    }
}

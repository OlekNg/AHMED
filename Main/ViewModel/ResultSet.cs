using Genetics.Statistics;
using OxyPlot;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Main.ViewModel
{
    [ImplementPropertyChanged]
    public class ResultSet
    {
        private string folderPath;
        private List<string> passes;

        public bool IsValid { get { return passes != null && passes.Count > 0; } }
        public List<List<IterationData>> IterationData { get; set; }
        public List<IterationDataWithDeviance> AvgFitness { get; set; }
        public List<IterationDataWithDeviance> BestChromosome { get; set; }

        public List<DataPoint> Selection { get; set; }
        public List<DataPoint> Crossover { get; set; }
        public List<DataPoint> Mutation { get; set; }
        public List<DataPoint> Repair { get; set; }
        public List<DataPoint> Transform { get; set; }
        public List<DataPoint> Evaluation { get; set; }
        public string Name { get; set; }
        public string FolderPath { get { return folderPath; } }

        public ResultSet(string folderPath)
        {
            this.folderPath = folderPath;
            ValidateFolder();
            if (IsValid)
            {
                SetResultSetName();
                LoadIterationsData();
                CalculateDevianceData();
            }
        }

        private void ValidateFolder()
        {
            passes = Directory.EnumerateDirectories(folderPath, "pass_*").ToList();
        }

        private void SetResultSetName()
        {
            Name = Path.GetFileNameWithoutExtension(folderPath);
        }

        private void LoadIterationsData()
        {
            IterationData = passes.Select(passFolder =>
            {
                var csv = new CsvImport<IterationData>();
                csv.Import(Path.Combine(passFolder, "iterations.csv"));
                return csv.Objects;
            }).ToList();
        }

        private void CalculateDevianceData()
        {
            AvgFitness = new List<IterationDataWithDeviance>();
            BestChromosome = new List<IterationDataWithDeviance>();
            Selection = new List<DataPoint>();
            Crossover = new List<DataPoint>();
            Mutation = new List<DataPoint>();
            Repair = new List<DataPoint>();
            Transform = new List<DataPoint>();
            Evaluation = new List<DataPoint>();

            var iterations = IterationData.First().Count;
            for (int i = 0; i < iterations; i++)
            {
                AvgFitness.Add(new IterationDataWithDeviance()
                {
                    NumberOfIteration = i,
                    Avg = IterationData.Average(x => x[i].AverageFitness),
                    Min = IterationData.Min(x => x[i].AverageFitness),
                    Max = IterationData.Max(x => x[i].AverageFitness)
                });

                BestChromosome.Add(new IterationDataWithDeviance()
                {
                    NumberOfIteration = i,
                    Avg = IterationData.Average(x => x[i].BestChromosomeValue),
                    Min = IterationData.Min(x => x[i].BestChromosomeValue),
                    Max = IterationData.Max(x => x[i].BestChromosomeValue)
                });

                Selection.Add(new DataPoint() { X = i, Y = IterationData.Average(x => x[i].SelectionOverhead) });
                Crossover.Add(new DataPoint() { X = i, Y = IterationData.Average(x => x[i].CrossoverOverhead) });
                Mutation.Add(new DataPoint() { X = i, Y = IterationData.Average(x => x[i].MutationOverhead) });
                Repair.Add(new DataPoint() { X = i, Y = IterationData.Average(x => x[i].RepairOverhead) });
                Transform.Add(new DataPoint() { X = i, Y = IterationData.Average(x => x[i].TransformOverhead) });
                Evaluation.Add(new DataPoint() { X = i, Y = IterationData.Average(x => x[i].EvaluationOverhead) });
            }
        }
    }
}

using Genetics;
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
        public List<IterationDataWithDeviance> IterationTime { get; set; }

        public List<DataPoint> SelectionOverhead { get; set; }
        public List<DataPoint> CrossoverOverhead { get; set; }
        public List<DataPoint> MutationOverhead { get; set; }
        public List<DataPoint> RepairOverhead { get; set; }
        public List<DataPoint> TransformOverhead { get; set; }
        public List<DataPoint> EvaluationOverhead { get; set; }
        public string Name { get; set; }
        public string FolderPath { get { return folderPath; } }

        public string Selection { get; set; }
        public string Crossover { get; set; }
        public string Mutation { get; set; }
        public string Transform { get; set; }
        public int PopulationSize { get; set; }
        public int MaxIterations { get; set; }
        public bool ShortGenotype { get; set; }
        public int MaxIterationsWithoutImprovement { get; set; }
        public string Iterations { get; set; }
        public int ChromosomeLength { get; set; }

        public double BestChromosomeValue { get; set; }

        public ResultSet(string folderPath)
        {
            this.folderPath = folderPath;
            ValidateFolder();
            if (IsValid)
            {
                SetResultSetName();
                LoadIterationsData();
                LoadGeneticsConfigurationData();
                LoadChromosomeLength();
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

        private void LoadGeneticsConfigurationData()
        {
            var reader = new EvacuationSimulation.XmlConfigurationReader(Path.Combine(folderPath, "Scenario.xml"));
            var cfg = reader.GetGeneticsConfiguration();
            Selection = cfg.Selector.ToString();
            Crossover = cfg.CrossoverOperator.ToString();
            Mutation = cfg.MutationOperator.ToString();
            Transform = cfg.Transformer != null ? cfg.Transformer.ToString() : "None";
            PopulationSize = cfg.InitialPopulationSize;
            MaxIterations = cfg.MaxIterations;
            ShortGenotype = cfg.ShortGenotype;
            MaxIterationsWithoutImprovement = cfg.MaxIterationsWithoutImprovement;
        }

        private void LoadChromosomeLength()
        {
            var genotype = File.ReadAllLines(Path.Combine(folderPath, "best_chromosome.txt"))[1];
            ChromosomeLength = genotype.Length;
        }

        private void CalculateDevianceData()
        {
            AvgFitness = new List<IterationDataWithDeviance>();
            BestChromosome = new List<IterationDataWithDeviance>();
            IterationTime = new List<IterationDataWithDeviance>();
            SelectionOverhead = new List<DataPoint>();
            CrossoverOverhead = new List<DataPoint>();
            MutationOverhead = new List<DataPoint>();
            RepairOverhead = new List<DataPoint>();
            TransformOverhead = new List<DataPoint>();
            EvaluationOverhead = new List<DataPoint>();

            BestChromosomeValue = IterationData.SelectMany(x => x.Select(y => y.BestChromosomeValue)).Max();
            Iterations = String.Join(", ", IterationData.Select(x => x.Count));

            var iterations = IterationData.Max(x => x.Count);
            for (int i = 0; i < iterations; i++)
            {
                var iterData = IterationData.Where(x => x.Count > i);

                AvgFitness.Add(new IterationDataWithDeviance()
                {
                    NumberOfIteration = i,
                    Avg = iterData.Average(x => x[i].AverageFitness),
                    Min = iterData.Min(x => x[i].AverageFitness),
                    Max = iterData.Max(x => x[i].AverageFitness)
                });

                BestChromosome.Add(new IterationDataWithDeviance()
                {
                    NumberOfIteration = i,
                    Avg = iterData.Average(x => x[i].BestChromosomeValue),
                    Min = iterData.Min(x => x[i].BestChromosomeValue),
                    Max = iterData.Max(x => x[i].BestChromosomeValue)
                });

                IterationTime.Add(new IterationDataWithDeviance()
                {
                    NumberOfIteration = i,
                    Avg = iterData.Average(x => x[i].IterationTimeInMillis),
                    Min = iterData.Min(x => x[i].IterationTimeInMillis),
                    Max = iterData.Max(x => x[i].IterationTimeInMillis)
                });

                SelectionOverhead.Add(new DataPoint() { X = i, Y = iterData.Average(x => x[i].SelectionOverhead) });
                CrossoverOverhead.Add(new DataPoint() { X = i, Y = iterData.Average(x => x[i].CrossoverOverhead) });
                MutationOverhead.Add(new DataPoint() { X = i, Y = iterData.Average(x => x[i].MutationOverhead) });
                RepairOverhead.Add(new DataPoint() { X = i, Y = iterData.Average(x => x[i].RepairOverhead) });
                TransformOverhead.Add(new DataPoint() { X = i, Y = iterData.Average(x => x[i].TransformOverhead) });
                EvaluationOverhead.Add(new DataPoint() { X = i, Y = iterData.Average(x => x[i].EvaluationOverhead) });
            }
        }
    }
}
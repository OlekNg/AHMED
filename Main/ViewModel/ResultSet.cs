using Genetics.Statistics;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Main.ViewModel
{
    public class ResultSet
    {
        private string folderPath;
        private List<string> passes;

        public bool IsValid { get { return passes != null && passes.Count > 0; } }
        public List<List<IterationData>> IterationData { get; set; }
        public List<IterationDataWithDeviance> AvgFitness { get; set; }
        public string Name { get; set; }

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
            }
        }
    }
}

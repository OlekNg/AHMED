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
        public List<IterationData> IterationData { get; set; }
        public string Name { get; set; }
        public List<DataPoint> AvgFitness { get; set; }

        public ResultSet(string folderPath)
        {
            this.folderPath = folderPath;
            ValidateFolder();
            if (IsValid)
            {
                SetResultSetName();
                LoadIterationsData();
                ConvertToDataPoints();
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
            var passFolder = passes.First();
            var csv = new CsvImport<IterationData>();
            csv.Import(Path.Combine(passFolder, "iterations.csv"));
            IterationData = csv.Objects;
        }

        private void ConvertToDataPoints()
        {
            AvgFitness = IterationData.Select(x => new DataPoint(x.NumberOfIteration, x.AverageFitness)).ToList();
        }
    }
}

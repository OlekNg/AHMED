using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Genetics.Statistics
{
    /// <summary>
    /// Wraps collecting statistics about running algorithm.
    /// </summary>
    class AlgorithmStatistics
    {
        private string _directory;
        private List<IterationData> _iterations;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        /// <param name="directory">Directory, where statistics data will be stored.</param>
        public AlgorithmStatistics(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _directory = directory;
            _iterations = new List<IterationData>();
        }

        public void Collect(GeneticAlgorithmStatus status)
        {
            _iterations.Add(new IterationData()
            {
                AverageFitness = status.CurrentPopulation.AvgFitness,
                PopulationFitness = status.CurrentPopulation.Fitness,
                NumberOfIteration = status.IterationNumber,
                BestChromosomeValue = status.BestChromosome.Value
            });
        }

        /// <summary>
        /// Dumps statistics to files.
        /// </summary>
        public void Dump()
        {
            CsvExport<IterationData> csvExport = new CsvExport<IterationData>(_iterations);
            csvExport.ExportToFile(Path.Combine(_directory, "iterations.csv"));
        }
    }
}

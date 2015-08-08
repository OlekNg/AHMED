using Genetics.Generic;
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
    public class AlgorithmStatistics
    {
        private string _directory;
        private double _bestChromosomeValue;
        private IChromosome _bestChromosome;
        private List<IterationData> _iterations;

        public double BestChromosomeValue { get { return _bestChromosomeValue; } }

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
            _bestChromosomeValue = Double.MinValue;
        }

        public void Collect(GeneticAlgorithmStatus status)
        {
            _iterations.Add(new IterationData()
            {
                AverageFitness = status.CurrentPopulation.AvgFitness,
                PopulationFitness = status.CurrentPopulation.Fitness,
                NumberOfIteration = status.IterationNumber,
                BestChromosomeValue = status.BestChromosome.Value,

                SelectionOverhead = status.SelectionOverhead,
                CrossoverOverhead = status.CrossoverOverhead,
                MutationOverhead = status.MutationOverhead,
                RepairOverhead = status.RepairOverhead,
                TransformOverhead = status.TransformOverhead,
                EvaluationOverhead = status.EvaluationOverhead
            });

            if (status.BestChromosome.Value > _bestChromosomeValue)
            {
                _bestChromosomeValue = status.BestChromosome.Value;
                _bestChromosome = status.BestChromosome.Clone();
            }
        }

        /// <summary>
        /// Dumps statistics to files.
        /// </summary>
        public void Dump()
        {
            CsvExport<IterationData> csvExport = new CsvExport<IterationData>(_iterations);
            csvExport.ExportToFile(Path.Combine(_directory, "iterations.csv"));

            var bestChromosome = _bestChromosome as Chromosome<List<bool>>;
            var genotype = new String(bestChromosome.Genotype.Select(x => x ? '1' : '0').ToArray());

            File.WriteAllLines(Path.Combine(_directory, "best_chromosome.txt"), new string[] { _bestChromosomeValue.ToString(), genotype });
        }
    }
}

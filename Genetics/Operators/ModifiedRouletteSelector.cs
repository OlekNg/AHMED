using Genetics.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Operators
{
    /// <summary>
    /// Modified roulette selector, that scales up population that each
    /// chromosome will have value greater than 0, and then scales using
    /// sigma-truncate method.
    /// </summary>
    public class ModifiedRouletteSelector : RouletteSelector
    {
        private const double c = 3;

        /// <summary>
        /// Population standard deviation.
        /// </summary>
        private double _sigma;

        public override Population Select(Population population)
        {
            // Search minimal chromosome value.
            double minValue = double.MaxValue;
            foreach (var chromosome in population.Chromosomes)
                if (chromosome.Value < minValue)
                    minValue = chromosome.Value;

            // Raise chromosome values if they are below 0.
            if (minValue < 0)
            {
                double value = Math.Abs(minValue);
                foreach (var chromosome in population.Chromosomes)
                    chromosome.Value += value;
            }

            population.UpdateStats();

            CalculateSigma(population);
            Scale(population);
            
            return base.Select(population);
        }

        private void Scale(Population population)
        {
            foreach (var chromosome in population.Chromosomes)
            {
                double value = chromosome.Value + (population.AvgFitness - c * _sigma);
                if (value < 0) value = 0;
                chromosome.Value = value;
            }
        }

        private void CalculateSigma(Population population)
        {
            double avg = population.AvgFitness;

            double sum = 0;
            foreach(var chromosome in population.Chromosomes)
                sum += Math.Pow(chromosome.Value - avg ,2);

            _sigma = Math.Sqrt(sum / population.Count);
        }
    }
}

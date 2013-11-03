using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Specialized
{
    public class RouletteSelector : ISelector
    {
        private Random _randomizer;

        public RouletteSelector()
        {
            _randomizer = new Random();
        }

        public Population Select(Population population)
        {
            population.Chromosomes.Sort();

            // Distribution function.
            List<double> F = new List<double>();

            double totalValue = 0;

            foreach (IChromosome c in population.Chromosomes)
                F.Add(totalValue += c.Value);

            // Normalize ( F belongs to <0,1>)
            for (int i = 0; i < F.Count; i++)
                F[i] /= totalValue;

            // Drawing chromosomes.
            Population result = new Population(population.Count);
            for (int i = 0; i < F.Count; i++)
            {
                double number = _randomizer.NextDouble();

                IChromosome selected = null;

                for (int j = 0; j < F.Count; j++)
                {
                    if (number < F[j])
                    {
                        selected = population[j].Clone();
                        break;
                    }
                }

                if (selected == null)
                    selected = population[population.Count - 1].Clone();

                result.Add(selected);
            }

            return result;
        }
    }
}

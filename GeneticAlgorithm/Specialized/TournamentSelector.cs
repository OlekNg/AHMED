using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genetics.Generic;

namespace Genetics.Specialized
{
    /// <summary>
    /// Deterministic tournament selector.
    /// </summary>
    public class TournamentSelector : ISelector
    {
        private Random _randomizer = new Random();

        public Population Select(Population population)
        {
            Population result = new Population(population.Count);

            // Tournament group
            List<IChromosome> group = new List<IChromosome>(2);

            for (int i = 0; i < population.Count; i++)
            {
                group.Clear();

                // Create group of two.
                group.Add(population[_randomizer.Next(0, population.Count)]);
                group.Add(population[_randomizer.Next(0, population.Count)]);

                // To-do evalutaing if population is not evaluated.

                // Add better from group.
                result.Add(group[0].Value > group[1].Value ? group[0].Clone() : group[1].Clone());
            }

            return result;
        }
    }
}

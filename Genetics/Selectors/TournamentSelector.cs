using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Selectors
{
    /// <summary>
    /// Deterministic tournament selector.
    /// </summary>
    public class TournamentSelector : ISelector
    {
        private Random _randomizer = new Random();

        public List<Chromosome> Select(List<Chromosome> population)
        {
            List<Chromosome> result = new List<Chromosome>(population.Count);

            // Tournament group
            List<Chromosome> group = new List<Chromosome>(2);

            for (int i = 0; i < population.Count; i++)
            {
                group.Clear();

                // Create group of two.
                group.Add(population[_randomizer.Next(0, population.Count)]);
                group.Add(population[_randomizer.Next(0, population.Count)]);

                // To-do evalutaing if population is not evaluated.

                // Add better from group.
                result.Add(group[0].Value > group[1].Value ? new Chromosome(group[0]) : new Chromosome(group[1]));
            }

            return result;
        }
    }
}

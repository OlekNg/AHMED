using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Selectors
{
    public class RouletteSelector : ISelector
    {
        private Random _randomizer;

        public RouletteSelector()
        {
            _randomizer = new Random();
        }

        public List<Chromosome> Select(List<Chromosome> population)
        {
            population.Sort();

            // Distribution function.
            List<double> F = new List<double>();

            double temp = 0;

            foreach (Chromosome c in population)
                F.Add(temp += c.Value);

            // Normalize ( F belongs to <0,1>
            for (int i = 0; i < F.Count; i++)
                F[i] /= temp;

            // Drawing chromosomes.
            List<Chromosome> result = new List<Chromosome>(population.Count);
            for (int i = 0; i < F.Count; i++)
            {
                double number = _randomizer.NextDouble();

                // Binary search - later

                Chromosome selected = null;

                for (int j = 0; j < F.Count; j++)
                {
                    if (number < F[j])
                    {
                        selected = new Chromosome(population[j]);
                        break;
                    }
                }

                if (selected == null)
                    selected = new Chromosome(population[population.Count - 1]);

                result.Add(selected);
            }

            return result;
        }
    }
}

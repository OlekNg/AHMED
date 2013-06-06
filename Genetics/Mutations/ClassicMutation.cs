using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Mutations
{
    public class ClassicMutation : IMutationOperator
    {
        private const double DEFAULT_PROBABILITY = 0.1;

        /// <summary>
        /// To randomize mutation.
        /// </summary>
        private Random randomizer;

        public ClassicMutation()
        {
            randomizer = new Random();
        }

        public void Mutate(Chromosome c)
        {
            Mutate(c, DEFAULT_PROBABILITY);
        }

        public void Mutate(Chromosome c, double probability)
        {
            List<bool> genotype = c.Genotype;
            for (int i = 0; i < genotype.Count; i++)
            {
                if (randomizer.NextDouble() <= probability)
                    genotype[i] = !genotype[i];
            }
        }
    }
}

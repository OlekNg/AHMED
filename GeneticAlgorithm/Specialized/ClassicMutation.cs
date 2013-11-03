using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Specialized
{
    public class ClassicMutation : IMutationOperator<List<bool>>
    {
        private const double DEFAULT_PROBABILITY = 0.1;

        /// <summary>
        /// To randomize mutation.
        /// </summary>
        private Random _randomizer;
        private double _probability;

        public ClassicMutation(double probability = DEFAULT_PROBABILITY)
        {
            _randomizer = new Random();
            _probability = probability;
        }

        public void Mutate(List<bool> genotype)
        {
            for (int i = 0; i < genotype.Count; i++)
            {
                if (_randomizer.NextDouble() <= _probability)
                    genotype[i] = !genotype[i];
            }
        }
    }
}

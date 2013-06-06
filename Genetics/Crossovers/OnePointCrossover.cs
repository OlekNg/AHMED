using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Crossovers
{
    class OnePointCrossover : ICrossoverOperator
    {
        /// <summary>
        /// To draw a slice point.
        /// </summary>
        private Random randomizer;

        public OnePointCrossover()
        {
            randomizer = new Random();
        }

        public Chromosome[] CrossAndCreate(Chromosome c1, Chromosome c2)
        {
            CheckChromosomes(c1, c2);
            List<bool>[] newGenotypes = CreateNewGenotypes(c1, c2);

            // Create new chromosomes.
            Chromosome[] result = new Chromosome[2];

            result[0].Genotype = newGenotypes[0];
            result[1].Genotype = newGenotypes[1];

            return result;
        }

        public void CrossAndReplace(Chromosome c1, Chromosome c2)
        {
            CheckChromosomes(c1, c2);
            List<bool>[] newGenotypes = CreateNewGenotypes(c1, c2);

            // Replace c1 and c2 genotypes.
            c1.Genotype = newGenotypes[0];
            c2.Genotype = newGenotypes[1];
        }

        /// <summary>
        /// Checks if crossing over is possible for given chromosomes.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        private void CheckChromosomes(Chromosome c1, Chromosome c2)
        {
            if (c1.Length != c2.Length)
                throw new CrossoverException("Illegal operation! One point crossover " +
                                             "cannot be done with different length chromosomes.");

            if (c1.Length < 2)
                throw new CrossoverException("Illegal operation! One point crossover " +
                                             "requires at least 2-long chromosome.");
        }

        /// <summary>
        /// Crosses genotypes from given chromosomes and creates
        /// recombined genotypes.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns>2 genotypes (2 element array of genotype).</returns>
        private List<bool>[] CreateNewGenotypes(Chromosome c1, Chromosome c2)
        {
            int length = c1.Length;

            // Slicing point.
            int point = randomizer.Next(1, length - 1);

            // Combine and create new genotypes.
            List<bool> g1 = c1.Genotype;
            List<bool> g2 = c2.Genotype;

            List<bool>[] newGenotypes = new List<bool>[2];

            // First genotype.
            newGenotypes[0].AddRange(g1.GetRange(0, point));
            newGenotypes[0].AddRange(g2.GetRange(point, length - point));

            // Second genotype.
            newGenotypes[1].AddRange(g2.GetRange(0, point));
            newGenotypes[1].AddRange(g1.GetRange(point, length - point));

            return newGenotypes;
        }
    }
}

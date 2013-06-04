using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics
{
    class Chromosome
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Chromosome() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="genotype">Initial genotype.</param>
        public Chromosome(bool[] genotype)
        {
            Genotype = genotype;
        }

        /// <summary>
        /// Binary chain.
        /// </summary>
        public bool[] Genotype { get; set; }

        /// <summary>
        /// Performs crossover with other chromosome
        /// using specific number of crossover points.
        /// </summary>
        /// <param name="points">Number of points to split the chromosome into.</param>
        /// <param name="c">Second chromosome to crossover.</param>
        /// <returns>New chromosome.</returns>
        public Chromosome Crossover(uint points, Chromosome chromosome)
        {
            return null;
        }

        /// <summary>
        /// Mutates chromosome.
        /// </summary>
        /// <param name="probability">Probability of single bit mutation.</param>
        public void Mutate(double probability)
        {
            return;
        }
    }
}

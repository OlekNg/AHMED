using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Crossovers
{
    /// <summary>
    /// Interface for crossover operators.
    /// </summary>
    public interface ICrossoverOperator
    {
        /// <summary>
        /// Performs crossover. Doesn't affect arguments.
        /// </summary>
        /// <param name="c1">First chromosome.</param>
        /// <param name="c2">Second chromosome.</param>
        /// <returns>Two new chromosomes.</returns>
        Chromosome[] CrossAndCreate(Chromosome c1, Chromosome c2);

        /// <summary>
        /// Performs crossover replacing original chromosomes.
        /// </summary>
        /// <param name="c1">First chromosome.</param>
        /// <param name="c2">Second chromosome.</param>
        void CrossAndReplace(Chromosome c1, Chromosome c2);
    }
}

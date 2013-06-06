using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Mutations
{
    /// <summary>
    /// Interface for mutation operators.
    /// </summary>
    public interface IMutationOperator
    {
        /// <summary>
        /// Performs mutation replacing original chromosomes.
        /// Uses default probability.
        /// </summary>
        /// <param name="c">Chromosome to mutate.</param>
        void Mutate(Chromosome c);

        /// <summary>
        /// Performs mutation replacing original chromosomes.
        /// </summary>
        /// <param name="c">Chromosome to mutate.</param>
        /// <param name="probability">Mutation probabilty.</param>
        void Mutate(Chromosome c, double probability);
    }
}

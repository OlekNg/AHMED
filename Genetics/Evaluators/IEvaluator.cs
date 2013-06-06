using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Evaluators
{
    /// <summary>
    /// Interface for evaluators.
    /// </summary>
    public interface IEvaluator
    {
        /// <summary>
        /// Evaluates chromosome.
        /// </summary>
        /// <param name="c">Chromosome to evaluate.</param>
        /// <returns>Value of evaluated chromosome.</returns>
        double Evaluate(Chromosome c);
    }
}

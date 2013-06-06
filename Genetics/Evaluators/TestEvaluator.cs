using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Evaluators
{
    /// <summary>
    /// Test evaluator.
    /// </summary>
    public class TestEvaluator : IEvaluator
    {
        public double Evaluate(Chromosome c)
        {
            List<bool> genotype = c.Genotype;
            double sum = 0;
            for (int i = 0; i < genotype.Count; i++)
            {
                sum += genotype[i] ? i : 0;
            }

            return sum;
        }
    }
}

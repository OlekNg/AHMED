using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics
{
    /// <summary>
    /// Genetic algorithm configuration for evacuation simulation.
    /// </summary>
    public class GeneticsConfiguration<T>
    {
        public ICrossoverOperator<T> CrossoverOperator { get; set; }
        public IMutationOperator<T> MutationOperator { get; set; }
        public ISelector Selector { get; set; }
        public ITransformer<T> Transformer { get; set; }
        public int InitialPopulationSize { get; set; }
        public int MaxIterations { get; set; }
        public bool ShortGenotype { get; set; }
    }
}

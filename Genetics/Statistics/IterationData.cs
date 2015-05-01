using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Statistics
{
    /// <summary>
    /// Data collected from single iteration.
    /// </summary>
    public class IterationData
    {
        public int NumberOfIteration { get; set; }
        public double AverageFitness { get; set; }
        public double PopulationFintness { get; set; }
        public double BestChromosomeValue { get; set; }
    }
}

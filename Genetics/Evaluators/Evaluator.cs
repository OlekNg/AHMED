using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Evaluators
{
    /// <summary>
    /// Calculates fitness function.
    /// It is a midclass between simulator and generation.
    /// </summary>
    class Evaluator : IEvaluator
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Evaluator() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="simulationFunction">Function that will be used to simulate
        /// chromosome.</param>
        public Evaluator(SimulationDelegate simulationFunction)
        {
            Simulate = simulationFunction;
        }

        /// <summary>
        /// Function type to delegate simulation.
        /// </summary>
        /// <param name="genotype">Genotype to simulate.</param>
        /// <returns>Simulation result - see ISimulationResult interface.</returns>
        /// <see cref="ISimulationResult"/>
        public delegate ISimulationResult SimulationDelegate(Chromosome c);

        public double Evaluate(Chromosome c)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Property for simulation function.
        /// <see cref="SimulationDelegate"/>
        /// </summary>
        public SimulationDelegate Simulate { get; set; }

        /// <summary>
        /// For testing purposes.
        /// </summary>
        /// <param name="genotype"></param>
        /// <returns></returns>
        public TestSimulationResult TestSimulationFunction(Chromosome c)
        {
            
            List<bool> genotype = c.Genotype;
            double sum = genotype.Count * genotype.Count + 2;
            for (int i = 0; i < genotype.Count; i++)
            {
                sum -= genotype[i] ? i : 0;
            }

            return new TestSimulationResult(sum, 10, false);
        }
    }
}

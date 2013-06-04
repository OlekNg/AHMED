using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics
{
    /// <summary>
    /// Calculates fitness function.
    /// It is a midclass between simulator and generation.
    /// </summary>
    class Evaluator
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
            SimulationFunction = simulationFunction;
        }

        /// <summary>
        /// Function type to delegate simulation.
        /// </summary>
        /// <param name="genotype">Genotype to simulate.</param>
        /// <returns>Simulation result - see ISimulationResult interface.</returns>
        /// <see cref="ISimulationResult"/>
        public delegate ISimulationResult SimulationDelegate(bool[] genotype);

        /// <summary>
        /// Chromosome that will be passed to simulator
        /// and evaluated afterwards.
        /// </summary>
        public Chromosome Chromosome { get; set; }

        /// <summary>
        /// Property for simulation function.
        /// <see cref="SimulationDelegate"/>
        /// </summary>
        public SimulationDelegate SimulationFunction { get; set; }
    }
}

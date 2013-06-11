using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulation;
using Structure;

namespace Genetics.Evaluators
{
    public class AHMEDEvaluator : IEvaluator
    {
        /// <summary>
        /// Maximum avarage escape time (it is simply amount of all building tiles).
        /// </summary>
        private double _maxAvgEscapeTime;

        /// <summary>
        /// Amount of the people in the building.
        /// </summary>
        private int _peopleCount;

        private Simulator _simulator;

        public AHMEDEvaluator(Simulator simulator, int poepleCount, BuildingMap bmap)
        {
            _simulator = simulator;
            _peopleCount = poepleCount;
            _maxAvgEscapeTime = bmap.Width * bmap.Height;
        }

        public double Evaluate(Chromosome c)
        {
            List<EscapedGroup> result = _simulator.Simulate(c.Fenotype);

            // Calculate avarage.
            double sum = 0;
            int sumTicks = 0;
            int peopleEscaped = 0;

            foreach (EscapedGroup g in result)
            {
                sum += g.Quantity * g.Ticks;
                sumTicks += (int)g.Ticks;
                peopleEscaped += (int)g.Quantity;
            }

            double avg;
            if (peopleEscaped != 16)
                avg = 0;
            else
            {
                avg = _maxAvgEscapeTime - (sum / (double)_peopleCount);
            }

            return (double)peopleEscaped + avg;
        }
    }
}

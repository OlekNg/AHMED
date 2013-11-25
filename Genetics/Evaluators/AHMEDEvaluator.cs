using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulation;
using Genetics.Generic;

namespace Genetics.Evaluators
{

    public class AHMEDEvaluator : IEvaluator<List<bool>>
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
        private BuildingEditor.Logic.Building _building;

        public AHMEDEvaluator(Simulator simulator, BuildingEditor.Logic.Building building)
        {
            _simulator = simulator;
            _building = building;
            _peopleCount = building.GetPeopleCount();
            _maxAvgEscapeTime = building.GetFloorCount() * 1.5;
        }

        public double Eval(List<bool> genotype)
        {
            _building.SetFenotype(genotype.ToFenotype());

            List<EscapedGroup> result = _simulator.Simulate(_building.GetSimulatorFenotype());

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
            if (peopleEscaped != _peopleCount)
                avg = 0;
            else
            {
                avg = _maxAvgEscapeTime - (sum / (double)_peopleCount);
            }

            return (double)peopleEscaped + avg;
        }
    }
}

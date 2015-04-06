using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulation;
using Genetics.Generic;
using BuildingEditor.ViewModel;
using Common.DataModel.Enums;
using System.Diagnostics;

namespace Genetics.Evaluators
{
    public class EvaCalcEvaluator : IEvaluator<List<bool>>
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
        private Building _building;

        private List<PeoplePath> _peoplePaths;
        private bool _debug;

        public EvaCalcEvaluator(Simulator simulator, Building building, bool debug = false)
        {
            _debug = debug;
            _simulator = simulator;
            _building = building;
            _maxAvgEscapeTime = building.GetFloorCount() * 1.5;
            _peopleCount = building.GetPeopleCount();

            _peoplePaths = _building.GetPeoplePaths();

            _building.UpdateFlow();
        }

        public double Eval(List<bool> genotype)
        {
            double avg = 0;
            double value = 0;
            _building.SetFenotype(genotype.ToFenotype());

            int peopleEscapedFromPaths = 0;
            foreach (var path in _peoplePaths)
            {
                path.Update();
                value -= path.LowestFlowValue;

                // Penalty for number of corners
                value -= (0.01 * path.Corners);

                if (path.SuccessfulEscape)
                    peopleEscapedFromPaths += path.PeopleCount;
            }

            value += peopleEscapedFromPaths;


            if (peopleEscapedFromPaths == _peopleCount)
            {
                // Calculate avarage.
                double sum = 0;
                int sumTicks = 0;
                int peopleEscaped = 0;

                List<EscapedGroup> result = _simulator.Simulate(_building.GetSimulatorFenotype());

                foreach (EscapedGroup g in result)
                {
                    sum += g.Quantity * g.Ticks;
                    sumTicks += g.Ticks;
                    peopleEscaped += g.Quantity;

                    if (_debug)
                        Console.WriteLine("Escaped group: {0} in {1} ticks", g.Quantity, g.Ticks);
                }

                if (peopleEscaped != peopleEscapedFromPaths)
                    throw new InvalidProgramException("People escaped from simulator doesn't match number of people from precalculation.");

                if (peopleEscapedFromPaths != _peopleCount)
                    avg = 0;
                else
                    avg = _maxAvgEscapeTime - (sum / (double)_peopleCount);
            }

            return value + avg;
        }
    }
}

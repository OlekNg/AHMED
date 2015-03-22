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

            CreateBuildingFlow();
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

        public void CreateBuildingFlow()
        {
            var entrances = FindBuildingEntrances();
            foreach (var segment in entrances)
                Flood(0, segment);
        }

        protected List<Segment> FindBuildingEntrances()
        {
            // Each segment which has available direction to null segment
            // can be considered as entrance to building.
            List<Segment> result = new List<Segment>();

            var floor = _building.Floors.Where(x => x.Level == 0).First();

            foreach (var row in floor.Segments)
            {
                foreach (var segment in row)
                {
                    var directions = segment.GetAvailableDirections();
                    foreach (var dir in directions)
                    {
                        if (segment.GetNeighbour(dir) == null)
                        {
                            result.Add(segment);
                            break;
                        }
                    }
                }
            }

            return result;
        }

        protected void Flood(int value, Segment segment)
        {
            // Stop recursion.
            if (segment.FlowValue <= value || segment.Type == SegmentType.NONE)
                return;

            // If stairs - then move to segment at exit of second stairs in pair.
            if (segment.Type == SegmentType.STAIRS)
            {
                Flood(value, segment.GetNextSegment());
                return;
            }

            // Update segment flow value.
            segment.FlowValue = value;

            // Flood available directions.
            var directions = segment.GetAvailableDirections();
            foreach (var dir in directions)
            {
                var next = segment.GetNeighbour(dir);
                if (next != null)
                    Flood(value + 1, next);
            }
        }
    }
}

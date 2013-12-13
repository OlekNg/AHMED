using BuildingEditor.ViewModel;
using Common.DataModel.Enums;
using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Operators
{
    /// <summary>
    /// Optimizes three-segments loops (when path loops along three subsequent segments).
    /// </summary>
    public class ThreeSegmentLoopOptimizer : ITransformer<List<bool>>
    {
        private const double DEFAULT_PROBABILITY = 1;

        private Random _randomizer = new Random();
        private Building _building;
        private List<PeoplePath> _paths;

        private Dictionary<Direction, Direction> _oppositeDirections;
        private double _probability = DEFAULT_PROBABILITY;

        public ThreeSegmentLoopOptimizer(Building building)
        {
            _building = new Building(building.ToDataModel());
            _paths = _building.GetPeoplePaths();

            _oppositeDirections = new Dictionary<Direction, Direction>();
            _oppositeDirections.Add(Direction.DOWN, Direction.UP);
            _oppositeDirections.Add(Direction.UP, Direction.DOWN);
            _oppositeDirections.Add(Direction.LEFT, Direction.RIGHT);
            _oppositeDirections.Add(Direction.RIGHT, Direction.LEFT);
        }

        public ThreeSegmentLoopOptimizer(Building building, double probability)
            : this(building)
        {
            _probability = probability;
        }

        #region ITransformer<List<bool>> Members

        public List<bool> Transform(Chromosome<List<bool>> c)
        {
            _building.SetFenotype(c.Genotype.ToFenotype());

            foreach (var path in _paths)
            {
                if (_randomizer.NextDouble() > _probability) continue;

                int pos = DetectLoop(path);
                if (pos >= 0)
                {
                    // Try optimizing through setting first segment
                    // to direction of second.
                    var segments = path.Segments;
                    var availableDirections = segments[pos].GetAvailableDirections();
                    var directionToSet = segments[pos + 1].Fenotype;

                    if (availableDirections.Contains(directionToSet))
                    {
                        segments[pos].Fenotype = directionToSet;
                        //Console.WriteLine("Optimized 3-segment loop!");
                    }
                }
            }

            return _building.GetFenotype().ToGenotype();
        }

        #endregion

        /// <summary>
        /// Detects three subsequent segments in given path that
        /// form loop.
        /// </summary>
        /// <param name="path">Path to analyze.</param>
        /// <returns>Index of first segment in three-segment loop. -1 if not detected.</returns>
        private int DetectLoop(PeoplePath path)
        {
            path.Update();
            var segments = path.Segments;

            for (int i = 0; i < segments.Count - 2; i++)
            {
                // Algorithm of detection - if there are three different directions
                // and first is opposite to third then loop detected.
                if (segments[i].Fenotype != segments[i + 1].Fenotype &&
                    segments[i].Fenotype != segments[i + 2].Fenotype &&
                    segments[i + 1].Fenotype != segments[i + 2].Fenotype &&
                    segments[i].Fenotype == _oppositeDirections[segments[i + 2].Fenotype])
                    return i;
            }

            return -1;
        }
    }
}

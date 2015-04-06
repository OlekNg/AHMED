using BuildingEditor.ViewModel;
using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Operators
{
    /// <summary>
    /// Mutation operator, that mutates chromosome by randomly changing
    /// direction one of the segment along the people path.
    /// </summary>
    public class PathDirectionMutation : IMutationOperator<List<bool>>
    {
        private const double DEFAULT_PROBABILITY = 0.01;

        private double _probability = DEFAULT_PROBABILITY;
        private Random _randomizer = new Random();

        private Building _building;
        private List<PeoplePath> _paths;

        public PathDirectionMutation(Building building)
        {
            // Do not operate on potentially view model building.
            _building = new Building(building);
            _paths = _building.GetPeoplePaths();
        }

        public PathDirectionMutation(Building building, double probability)
            : this(building)
        {
            _probability = probability;
        }

        #region IMutationOperator<List<bool>> Members

        public List<bool> Mutate(Chromosome<List<bool>> c)
        {
            _building.SetFenotype(c.Genotype.ToFenotype());

            // For each path check probability of mutation
            foreach (var path in _paths)
            {
                if (_randomizer.NextDouble() > _probability)
                    continue;

                // Update and draw point of mutation.
                path.Update();
                var segments = path.Segments;
                int index = _randomizer.Next(0, segments.Count);
                var segment = segments[index];

                // Change direction to another available direction.
                var directions = segment.GetAvailableDirections(segment.Fenotype);
                if (directions.Count > 0)
                    segment.Fenotype = directions[_randomizer.Next(0, directions.Count)];
            }

            return _building.GetFenotype().ToGenotype();
        }

        #endregion
    }
}

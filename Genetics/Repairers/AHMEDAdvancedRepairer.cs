using BuildingEditor.Logic;
using Common.DataModel.Enums;
using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Repairers
{
    /// <summary>
    /// Repairer for AHMED. Repairs chromosomes pointing to the walls
    /// and adjacent small loops.
    /// </summary>
    public class AdvancedRepairer : IRepairer<List<bool>>
    {
        private Building _building;
        private Random _randomizer = new Random();
        private Dictionary<Direction, Direction> _oppositeDirections;

        public AdvancedRepairer(Building building)
        {
            _building = building;

            _oppositeDirections = new Dictionary<Direction, Direction>();
            _oppositeDirections.Add(Direction.DOWN, Direction.UP);
            _oppositeDirections.Add(Direction.UP, Direction.DOWN);
            _oppositeDirections.Add(Direction.LEFT, Direction.RIGHT);
            _oppositeDirections.Add(Direction.RIGHT, Direction.LEFT);
        }

        public List<bool> Repair(Chromosome<List<bool>> c)
        {
            var genotype = c.Genotype;
            // Set fenotype to building that we can analyze fenotype on actual building.
            _building.SetFenotype(genotype.ToFenotype());

            // Repair each floor
            foreach (var floor in _building.Floors)
            {
                // First phase - repair pointing at walls.
                foreach (var row in floor.Segments)
                {
                    foreach (var segment in row)
                    {
                        // If fenotype points at the wall, then change it to available direction.
                        if (segment.GetSideElement(segment.Fenotype).Type == SideElementType.WALL)
                        {
                            var availableDirections = GetAvailableDirections(segment);
                            if (availableDirections.Count == 0)
                                throw new Exception("Cannot fix wall pointing fenotype - invalid building? (No direction available).");

                            // Select randomly new direction (fenotype).
                            Direction newDirection = availableDirections[_randomizer.Next(0, availableDirections.Count)];
                            segment.Fenotype = newDirection;
                        }
                    }
                }

                List<Segment> fixedSegments = new List<Segment>();

                // Repair small loops.
                foreach (var row in floor.Segments)
                {
                    foreach (var segment in row)
                    {
                        var dir = segment.Fenotype;
                        var opposite = _oppositeDirections[dir];

                        var neighbour = segment.GetNeighbour(dir);

                        // If neighbour that we are pointing at is pointing at us then
                        // we have to fix that loop.
                        if (neighbour != null && neighbour.Fenotype == opposite)
                            FixSmallLoop(segment, neighbour, fixedSegments);
                    }
                }
            }

            return _building.GetFenotype().ToGenotype();
        }

        protected void FixSmallLoop(Segment segment1, Segment segment2, List<Segment> fixedSegments)
        {
            // Firstly try to fix from segment1 perspective, then from segment2.
            if (!fixedSegments.Contains(segment1))
            {
                var availableDirections = GetAvailableDirections(segment1, segment1.Fenotype);

                // If there are available directions then we can fix it.
                // Otherwise try to fix it from segment2 perspective.
                if (availableDirections.Count > 0)
                {
                    segment1.Fenotype = availableDirections[_randomizer.Next(0, availableDirections.Count)];
                    fixedSegments.Add(segment1);
                    return;
                }
            }

            if (!fixedSegments.Contains(segment2))
            {
                var availableDirections = GetAvailableDirections(segment2, segment2.Fenotype);

                // If there are available directions then we can fix it.
                // Otherwise cannot be fixed.
                if (availableDirections.Count > 0)
                {
                    segment2.Fenotype = availableDirections[_randomizer.Next(0, availableDirections.Count)];
                    fixedSegments.Add(segment2);
                    return;
                }
            }
        }

        /// <summary>
        /// Checks for available directions from given segment.
        /// It excludes directions that lead to walls.
        /// Does not excludes directions that might lead to another small adjacent loop.
        /// (it allows that change - change is good for genetic algorithm).
        /// </summary>
        /// <param name="segment">Considered segment.</param>
        /// <returns>List of available directions.</returns>
        protected List<Direction> GetAvailableDirections(Segment segment)
        {
            List<Direction> result = new List<Direction>() { Direction.LEFT, Direction.UP, Direction.RIGHT, Direction.DOWN };

            var sideElements = segment.GetSideElements();
            var neighbours = segment.GetNeighbours();

            foreach (Direction side in typeof(Direction).GetEnumValues())
            {
                // Doors are ok - analyze next direction.
                if (sideElements[side].Type == SideElementType.DOOR)
                    continue;

                // If we encount wall - remove from available directions.
                if (sideElements[side].Type == SideElementType.WALL)
                {
                    result.Remove(side);
                    continue;
                }

                // Remove direction that leads to segment of type none
                // (note we DO NOT exclude null segments - they are considered
                // as escape from building.
                if (neighbours[side].Type == SegmentType.NONE)
                    result.Remove(side);
            }

            return result;
        }

        /// <param name="except">Excludes explicitly one direction from result.</param>
        protected List<Direction> GetAvailableDirections(Segment segment, Direction except)
        {
            List<Direction> result = GetAvailableDirections(segment);
            result.Remove(except);
            return result;
        }
    }
}

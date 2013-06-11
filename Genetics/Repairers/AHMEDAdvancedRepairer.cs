using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;

namespace Genetics.Repairers
{
    /// <summary>
    /// Repairs small loops (neighbouring tiles).
    /// </summary>
    public class AHMEDAdvancedRepairer : IRepairer
    {
        private BuildingMap _bmap;

        private int _expectedChromosomeLength;

        /// <summary>
        /// Indicates if specified part of chromosome has been already reapired.
        /// Every element corresponds to one fenotype element.
        /// </summary>
        private bool[] _fixedTiles;

        private Random _randomizer = new Random();

        public AHMEDAdvancedRepairer(BuildingMap bmap)
        {
            _bmap = bmap;
            _expectedChromosomeLength = (int)_bmap.Height * (int)_bmap.Width * 2;
            _fixedTiles = new bool[(int)_bmap.Height * (int)_bmap.Width];
            ResetFixedTilesArray();
        }

        public void RepairAndReplace(Chromosome c)
        {
            if (c.Length != _expectedChromosomeLength)
                throw new RepairerException("Chromosome does not match the building map. Different length.");

            ResetFixedTilesArray();

            List<Direction> fenotype = c.Fenotype;

            // Fixing walls.
            // Check every tile. If sign points at wall then
            // change to another direction.
            for (int i = 0; i < _bmap.Height; i++)
            {
                for (int j = 0; j < _bmap.Width; j++)
                {
                    int fenotypeIndex = i * (int)_bmap.Width + j;

                    IWallElement element;
                    element = _bmap.Floor[i][j].GetSide(fenotype[fenotypeIndex]);

                    if(element.CanPassThrough)
                        continue;

                    // People cannot pass (wall)
                    // Find available direction.
                    List<Direction> availableDirections = GetAvailableDirections(i, j);

                    if(availableDirections.Count == 0)
                        throw new RepairerException("Cannot fix chromosome. There is a tile with no available direction.");

                    // Set new direction choosing one of available randomly.
                    c.SetFenotype(fenotypeIndex, availableDirections[_randomizer.Next(0, availableDirections.Count)]);
                }
            }

            // Refresh fenotype.
            fenotype = c.Fenotype;

            // Fixing small loops.
            for (int i = 0; i < _bmap.Height; i++)
            {
                for (int j = 0; j < _bmap.Width; j++)
                {
                    int fenotypeIndex = i * (int)_bmap.Width + j;
                    int secondFenotypeIndex;

                    switch (fenotype[fenotypeIndex])
                    {
                        case Direction.DOWN:
                            // Make sure we are still in the building
                            secondFenotypeIndex = fenotypeIndex + (int)_bmap.Width;
                            if (secondFenotypeIndex < fenotype.Count)
                            {
                                if (fenotype[secondFenotypeIndex] == Direction.UP)
                                    FixSmallLoop(fenotypeIndex, secondFenotypeIndex, fenotype, c);
                            }
                            break;
                            
                        case Direction.UP:
                            // Make sure we are still in the building
                            secondFenotypeIndex = fenotypeIndex - (int)_bmap.Width;
                            if (secondFenotypeIndex >= 0)
                            {
                                if (fenotype[secondFenotypeIndex] == Direction.DOWN)
                                    FixSmallLoop(fenotypeIndex, secondFenotypeIndex, fenotype, c);
                            }
                            break;

                        case Direction.LEFT:
                            // Make sure we are still in the building
                            secondFenotypeIndex = fenotypeIndex - 1;
                            if (secondFenotypeIndex >= 0)
                            {
                                if (fenotype[secondFenotypeIndex] == Direction.RIGHT)
                                    FixSmallLoop(fenotypeIndex, secondFenotypeIndex, fenotype, c);
                            }
                            break;

                        case Direction.RIGHT:
                            // Make sure we are still in the building
                            secondFenotypeIndex = fenotypeIndex + 1;
                            if (secondFenotypeIndex < fenotype.Count)
                            {
                                if (fenotype[secondFenotypeIndex] == Direction.LEFT)
                                    FixSmallLoop(fenotypeIndex, secondFenotypeIndex, fenotype, c);
                            }
                            break;

                        default:
                            break;
                    }

                }
            }
        }

        public Chromosome RepairAndCreate(Chromosome c)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Repairs small loop (of two neighbouring tiles).
        /// </summary>
        /// <param name="firstFenotypeIndex"></param>
        /// <param name="secondFenotypeIndex"></param>
        /// <param name="fenotype">It will be updated - to avoid fenotype
        /// creating during every small loop.</param>
        /// <param name="c">Chromosome to update.</param>
        private void FixSmallLoop(int firstFenotypeIndex, int secondFenotypeIndex, List<Direction> fenotype, Chromosome c)
        {
            // If both tiles has been fixed till now - don't do anything.
            if (_fixedTiles[firstFenotypeIndex] && _fixedTiles[secondFenotypeIndex])
                return;

            // Get coordinates.
            int i1 = firstFenotypeIndex / (int)_bmap.Width;
            int j1 = firstFenotypeIndex % (int)_bmap.Width;

            int i2 = secondFenotypeIndex / (int)_bmap.Width;
            int j2 = secondFenotypeIndex % (int)_bmap.Width;

            // Get other available directions for each tile.
            List<Direction> firstAvailableDirections = GetAvailableDirections(i1, j1, fenotype[firstFenotypeIndex]);
            List<Direction> secondAvailableDirections = GetAvailableDirections(i2, j2, fenotype[secondFenotypeIndex]);

            // If there are no possibilities to fix this loop - leave it.
            if (firstAvailableDirections.Count == 0 && secondAvailableDirections.Count == 0)
                return;

            if (_fixedTiles[firstFenotypeIndex] || _fixedTiles[secondFenotypeIndex])
            {
                // Only one tile is not fixed yet - try to fix it.
                if (_fixedTiles[firstFenotypeIndex])
                {
                    // Second tile.
                    if (secondAvailableDirections.Count > 0)
                    {
                        Direction newDirection = secondAvailableDirections[_randomizer.Next(0, secondAvailableDirections.Count)];
                        c.SetFenotype(secondFenotypeIndex, newDirection);
                        fenotype[secondFenotypeIndex] = newDirection;
                        _fixedTiles[secondFenotypeIndex] = true;
                    }
                }
                else
                {
                    // First tile.
                    if (firstAvailableDirections.Count > 0)
                    {
                        Direction newDirection = firstAvailableDirections[_randomizer.Next(0, firstAvailableDirections.Count)];
                        c.SetFenotype(firstFenotypeIndex, newDirection);
                        fenotype[firstFenotypeIndex] = newDirection;
                        _fixedTiles[firstFenotypeIndex] = true;
                    }
                }
            }
            else
            {
                // Both tiles are available to fix - choose randomly if both have
                // available directions and try to fix it.
                if (firstAvailableDirections.Count > 0 && secondAvailableDirections.Count > 0)
                {
                    if (_randomizer.Next(0, 2) == 0)
                    {
                        // First tile
                        Direction newDirection = firstAvailableDirections[_randomizer.Next(0, firstAvailableDirections.Count)];
                        c.SetFenotype(firstFenotypeIndex, newDirection);
                        fenotype[firstFenotypeIndex] = newDirection;
                        _fixedTiles[firstFenotypeIndex] = true;
                    }
                    else
                    {
                        // Second tile
                        Direction newDirection = secondAvailableDirections[_randomizer.Next(0, secondAvailableDirections.Count)];
                        c.SetFenotype(secondFenotypeIndex, newDirection);
                        fenotype[secondFenotypeIndex] = newDirection;
                        _fixedTiles[secondFenotypeIndex] = true;
                    }
                }
                else
                {
                    // Only one have available directions - fix it.
                    if (firstAvailableDirections.Count > 0)
                    {
                        // First tile
                        Direction newDirection = firstAvailableDirections[_randomizer.Next(0, firstAvailableDirections.Count)];
                        c.SetFenotype(firstFenotypeIndex, newDirection);
                        fenotype[firstFenotypeIndex] = newDirection;
                        _fixedTiles[firstFenotypeIndex] = true;
                    }
                    else
                    {
                        // Second tile
                        Direction newDirection = secondAvailableDirections[_randomizer.Next(0, secondAvailableDirections.Count)];
                        c.SetFenotype(secondFenotypeIndex, newDirection);
                        fenotype[secondFenotypeIndex] = newDirection;
                        _fixedTiles[secondFenotypeIndex] = true;
                    }
                }
            }

        }

        /// <summary>
        /// Checks for available directions for given tile coordinates
        /// </summary>
        /// <param name="i">Row</param>
        /// <param name="j">Column</param>
        /// <returns>List of available directions.</returns>
        private List<Direction> GetAvailableDirections(int i, int j)
        {
            List<Direction> result = new List<Direction>();

            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                if (_bmap.Floor[i][j].GetSide(d).CanPassThrough)
                    result.Add(d);
            }

            return result;
        }

        /// <summary>
        /// Checks for available directions for given tile coordinates
        /// </summary>
        /// <param name="i">Row</param>
        /// <param name="j">Column</param>
        /// <param name="except">Excluded direction from result.</param>
        /// <returns>List of available directions.</returns>
        private List<Direction> GetAvailableDirections(int i, int j, Direction except)
        {
            List<Direction> result = GetAvailableDirections(i, j);

            result.Remove(except);

            return result;
        }

        /// <summary>
        /// Resets whole _fixedTiles array to false.
        /// </summary>
        private void ResetFixedTilesArray()
        {
            for (int i = 0; i < _fixedTiles.Length; i++)
                _fixedTiles[i] = false;
        }
    }
}

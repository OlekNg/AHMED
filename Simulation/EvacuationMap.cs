using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;
using Simulation.Exceptions;

namespace Simulation
{
    /// <summary>
    /// Class containing all necessary information about evacuation map
    /// </summary>
    public class EvacuationMap
    {
        /// <summary>
        /// Width of map
        /// </summary>
        private uint _width;

        /// <summary>
        /// Height of map
        /// </summary>
        private uint _height;

        /// <summary>
        /// Map containing evacuation routes and current situation
        /// </summary>
        private EvacuationElement[][] _map;

        /// <summary>
        /// Check if provided coordinates belongs to this evacuation map
        /// </summary>
        /// <param name="row">Row 0 indexed</param>
        /// <param name="col">Column 0 indexed</param>
        /// <returns>True if coordinates belong to e. m., false - otherwise</returns>
        private bool CheckRanges(uint row, uint col)
        {
            return !(row >= _height || col >= _width);
        }

        /// <summary>
        /// Get evacuation route element with given coordinates
        /// </summary>
        /// <param name="row">Row (0 indexed)</param>
        /// <param name="col">Column (0 indexed)</param>
        /// <returns>Return evacuation element with given coords, null if such coordinates are outside map</returns>
        public EvacuationElement Get(uint row, uint col)
        {
            if (CheckRanges(row, col)) return _map[row][col];
            return null;
        }

        /// <summary>
        /// Method initializes evacuation map from given buulding map
        /// </summary>
        /// <param name="bm">Building map</param>
        public void InitializeFromBuildingMap(BuildingMap bm)
        {
            _width = bm.Width;
            _height = bm.Height;

            _map = new EvacuationElement[bm.Height][];
            for (uint i = 0; i < bm.Height; ++i)
            {
                _map[i] = new EvacuationElement[bm.Width];
                for (int j = 0; j < bm.Width; ++j)
                {
                    if(bm.Floor[i][j] != null)
                        _map[i][j] = new EvacuationElement(bm.Floor[i][j]);
                }
            }
        }

        /// <summary>
        /// Methos places given people group in evacuation map
        /// </summary>
        /// <param name="group">People group</param>
        public void SetPeopleGroup(PeopleGroup group)
        {
            _map[group.Row][group.Col].PeopleQuantity = group.Quantity;
        }

        /// <summary>
        /// Reset whole map (set PeopleQuantity as 0 and Processed as false)
        /// </summary>
        public void ResetPeopleGroups()
        {
            foreach (EvacuationElement[] e in _map)
                foreach (EvacuationElement element in e)
                {
                    element.PeopleQuantity = 0;
                    element.Processed = false;
                }
        }

        /// <summary>
        /// Initalize NextStep and Passage properties (evacuation routes in fact) from given fenotype
        /// </summary>
        /// <param name="fenotype">Given fenotype</param>
        public void MapFenotype(List<Direction> fenotype)
        {
            var fenotypeEnumerator = fenotype.GetEnumerator();
            for (uint i = 0; i < _height; ++i)
            {
                for (uint j = 0; j < _width; ++j)
                {
                    EvacuationElement element = Get(i, j);

                    if (element == null) continue;

                    if (!fenotypeEnumerator.MoveNext())
                    {
                        throw new BadFenotypeLengthException();
                    }

                    Direction direction = fenotypeEnumerator.Current;

                    element.Passage = element.FloorSquare.GetSide(direction);

                    switch (direction)
                    {
                        case Direction.DOWN:
                            element.NextStep = Get(i + 1, j);
                            break;
                        case Direction.UP:
                            element.NextStep = Get(i - 1, j);
                            break;
                        case Direction.LEFT:
                            element.NextStep = Get(i, j - 1);
                            break;
                        case Direction.RIGHT:
                            element.NextStep = Get(i, j + 1);
                            break;
                    }
                }
            }
        }
    }
}

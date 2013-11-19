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
        private List<uint> _width;

        /// <summary>
        /// Height of map
        /// </summary>
        private List<uint> _height;

        private int _floors;

        /// <summary>
        /// Map containing evacuation routes and current situation
        /// </summary>
        private List<EvacuationElement[][]> _map;

        /// <summary>
        /// Check if provided coordinates belongs to this evacuation map
        /// </summary>
        /// <param name="row">Row 0 indexed</param>
        /// <param name="col">Column 0 indexed</param>
        /// <returns>True if coordinates belong to e. m., false - otherwise</returns>
        private bool CheckRanges(int floor, uint row, uint col)
        {
            return !(floor <0 || floor >= _floors || row >= _height[floor] || col >= _width[floor]);
        }

        /// <summary>
        /// Get evacuation route element with given coordinates
        /// </summary>
        /// <param name="row">Row (0 indexed)</param>
        /// <param name="col">Column (0 indexed)</param>
        /// <returns>Return evacuation element with given coords, null if such coordinates are outside map</returns>
        public EvacuationElement Get(int floor, uint row, uint col)
        {
            if (CheckRanges(floor, row, col)) return _map[floor][row][col];
            return null;
        }

        public EvacuationElement Get(WallElementPosition wep)
        {
            return Get((int) wep.Floor, wep.Row, wep.Col);
        }

        /// <summary>
        /// Method initializes evacuation map from given buulding map
        /// </summary>
        /// <param name="bm">Building map</param>
        public void InitializeFromBuildingMap(BuildingMap bm)
        {
            uint w, h;
            EvacuationElement[][] temp;
            FloorSquare fs;

            _floors = bm.Floors.Count;
            _width = new List<uint>(_floors);
            _height = new List<uint>(_floors);
            _map = new List<EvacuationElement[][]>();


            for (int i = 0; i < _floors; ++i)
            {
                w = bm.Floors[i].Width;
                h = bm.Floors[i].Height;

                _width.Add(w);
                _height.Add(h);

                temp = new EvacuationElement[h][];
                for (uint j = 0; j < h; ++j)
                {
                    temp[j] = new EvacuationElement[w];

                    for (uint k = 0; k < w; ++k)
                    {
                        if ((fs = bm.GetSquare(i, j, k)) != null)
                            temp[j][k] = new EvacuationElement(fs);
                    }
                    _map.Add(temp);
                }
            }

            /*
             * OLD
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
             * */
        }

        /// <summary>
        /// Methos places given people group in evacuation map
        /// </summary>
        /// <param name="group">People group</param>
        public void SetPeopleGroup(PeopleGroup group)
        {
            //_map[group.Floor][group.Row][group.Col].PeopleQuantity = group.Quantity;
            _map[group.Floor][group.Row][group.Col].Setup(group.Quantity);
        }

        /// <summary>
        /// Reset whole map (set PeopleQuantity as 0 and Processed as false)
        /// </summary>
        public void ResetPeopleGroups()
        {
            foreach(EvacuationElement[][] ee in _map)
                foreach (EvacuationElement[] e in ee)
                    foreach (EvacuationElement element in e)
                    {
                        //element.PeopleQuantity = 0;
                        //;element.Processed = false;
                        element.Setup(0);
                    }
        }

        /// <summary>
        /// Initalize NextStep and Passage properties (evacuation routes in fact) from given fenotype
        /// </summary>
        /// <param name="fenotype">Given fenotype</param>
        public void MapFenotype(List<List<Direction>> fenotype)
        {
            List<Direction>.Enumerator fenotypeEnumerator;

            for (int i = 0; i < _floors; ++i)
            {
                fenotypeEnumerator = fenotype[i].GetEnumerator();

                for (uint j = 0; j < _height[i]; ++j)
                {
                    for (uint k = 0; k < _width[i]; ++k)
                    {
                        EvacuationElement element = Get(i, j, k);

                        if (element == null) continue;

                        if (!fenotypeEnumerator.MoveNext())
                        {
                            throw new BadFenotypeLengthException();
                        }

                        Direction direction = fenotypeEnumerator.Current;

                        element.Passage = element.FloorSquare.GetSide(direction);

                        if (element.Passage.Type == WallElementType.STAIR_ENTRY)
                        {
                            EvacuationElement stairsElement = new StairsEvacuationElement((StairsEntry) element.Passage, this);
                            element.NextStep = stairsElement;
                        }
                        else
                        {
                            switch (direction)
                            {
                                case Direction.DOWN:
                                    element.NextStep = Get(i, j + 1, k);
                                    break;
                                case Direction.UP:
                                    element.NextStep = Get(i, j - 1, k);
                                    break;
                                case Direction.LEFT:
                                    element.NextStep = Get(i, j, k - 1);
                                    break;
                                case Direction.RIGHT:
                                    element.NextStep = Get(i, j, k + 1);
                                    break;
                            }
                        }  
                    }
                }
            }
        }
    }
}

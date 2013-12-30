using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;
using Simulation.Exceptions;
using Common.DataModel.Enums;

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
        //private List<int> _width;

        /// <summary>
        /// Height of map
        /// </summary>
        //private List<int> _height;

        //private int _floors;

        /// <summary>
        /// Map containing evacuation routes and current situation
        /// </summary>
        //private List<EvacuationElement[][]> _map;
        private IDictionary<int, IDictionary<int, IDictionary<int, EvacuationElement>>> _map;

        /// <summary>
        /// Check if provided coordinates belongs to this evacuation map
        /// </summary>
        /// <param name="row">Row 0 indexed</param>
        /// <param name="col">Column 0 indexed</param>
        /// <returns>True if coordinates belong to e. m., false - otherwise</returns>
        private bool CheckRanges(int floor, int row, int col)
        {
            return !(floor < 0 || row < 0 || col < 0/* || floor >= _floors || row >= _height[floor] || col >= _width[floor]*/);
        }

        /// <summary>
        /// Get evacuation route element with given coordinates
        /// </summary>
        /// <param name="row">Row (0 indexed)</param>
        /// <param name="col">Column (0 indexed)</param>
        /// <returns>Return evacuation element with given coords, null if such coordinates are outside map</returns>
        /*public EvacuationElement Get(int floor, int row, int col)
        {
            if (CheckRanges(floor, row, col)) return _map[floor][row][col];
            return null;
        }*/
        public EvacuationElement Get(int floor, int row, int col)
        {
            IDictionary<int, IDictionary<int, EvacuationElement>> floorDict;
            IDictionary<int, EvacuationElement> rowDict;
            EvacuationElement result;

            if (!_map.TryGetValue(floor, out floorDict))
                return null;

            if (!floorDict.TryGetValue(row, out rowDict))
                return null;

            if (!rowDict.TryGetValue(col, out result))
                result = null;

            return result;
        }

        public EvacuationElement Get(WallElementPosition wep)
        {
            return Get(wep.Floor.Number, wep.Row, wep.Col);
        }

        /// <summary>
        /// Method initializes evacuation map from given buulding map
        /// </summary>
        /// <param name="bm">Building map</param>
        public void InitializeFromBuildingMap(BuildingMap bm)
        {
            //int w, h;
            //EvacuationElement[][] temp;
            //Tile fs;

            //_floors = bm.Floors.Count;
            //_width = new List<int>(_floors);
            //_height = new List<int>(_floors);
            //_map = new List<EvacuationElement[][]>();
            _map = new SortedDictionary<int, IDictionary<int, IDictionary<int, EvacuationElement>>>();


            //for (int i = 0; i < _floors; ++i)
            foreach(var e in bm.Floors)
            {
                Floor f = e.Value;
                int level = e.Key;
                //w = bm.Floors[i].Width;
                //h = bm.Floors[i].Height;

                //_width.Add(w);
                //_height.Add(h);

                //temp = new EvacuationElement[h][];
                
                IDictionary<int, IDictionary<int, EvacuationElement>> floorMap = new SortedDictionary<int, IDictionary<int, EvacuationElement>>();

                /*for (int j = 0; j < h; ++j)
                {
                    temp[j] = new EvacuationElement[w];

                    for (int k = 0; k < w; ++k)
                    {
                        if ((fs = bm.GetSquare(i, j, k)) != null)
                            temp[j][k] = new EvacuationElement(fs);
                    }
                }*/
                foreach (var row in f.Tiles)
                {
                    IDictionary<int, EvacuationElement> tempRow = new SortedDictionary<int, EvacuationElement>();
                    foreach (var tile in row.Value)
                    {
                        tempRow.Add(tile.Key, new EvacuationElement(tile.Value));
                    }
                    floorMap.Add(row.Key, tempRow);
                }
                //_map.Add(temp);
                _map.Add(level, floorMap);
            }

            /*
             * OLD
            _width = bm.Width;
            _height = bm.Height;

            _map = new EvacuationElement[bm.Height][];
            for (int i = 0; i < bm.Height; ++i)
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
            foreach(var ee in _map)
                foreach (var e in ee.Value)
                    foreach (var element in e.Value)
                    {
                        //element.PeopleQuantity = 0;
                        //;element.Processed = false;

                        //if(element != null)
                        element.Value.Setup(0);
                    }
        }

        /// <summary>
        /// Initalize NextStep and Passage properties (evacuation routes in fact) from given fenotype
        /// </summary>
        /// <param name="fenotype">Given fenotype</param>
        public void MapFenotype(List<List<Direction>> fenotype)
        {
            var buildingFenotypeEnumerator = fenotype.GetEnumerator();
            List<Direction>.Enumerator fenotypeEnumerator;

            //for (int i = 0; i < _floors; ++i)
            foreach(var floorMap in _map)
            {
                if (!buildingFenotypeEnumerator.MoveNext())
                {
                    throw new BadFenotypeLengthException();
                }

                fenotypeEnumerator = buildingFenotypeEnumerator.Current.GetEnumerator();

                //for (int j = 0; j < _height[i]; ++j)
                foreach(var row in floorMap.Value)
                {
                    //for (int k = 0; k < _width[i]; ++k)
                    foreach(var tile in row.Value)
                    {
                        //EvacuationElement element = Get(i, j, k);
                        EvacuationElement element = tile.Value;

                        if (!fenotypeEnumerator.MoveNext())
                        {
                            throw new BadFenotypeLengthException();
                        }

                        //if (element == null) continue;

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
                                    element.NextStep = Get(floorMap.Key, row.Key + 1, tile.Key);
                                    break;
                                case Direction.UP:
                                    element.NextStep = Get(floorMap.Key, row.Key - 1, tile.Key);
                                    break;
                                case Direction.LEFT:
                                    element.NextStep = Get(floorMap.Key, row.Key, tile.Key - 1);
                                    break;
                                case Direction.RIGHT:
                                    element.NextStep = Get(floorMap.Key, row.Key, tile.Key + 1);
                                    break;
                            }
                        }  
                    }
                }
            }
        }
    }
}

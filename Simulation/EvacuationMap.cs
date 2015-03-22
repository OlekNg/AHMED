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
        /// Map containing evacuation routes and current situation
        /// </summary>
        private IDictionary<int, IDictionary<int, IDictionary<int, EvacuationElement>>> _map;

        /// <summary>
        /// Evacuation elements whose next step is outside of the building (null).
        /// </summary>
        public List<EvacuationElement> Exits;

        /// <summary>
        /// Get evacuation route element with given coordinates
        /// </summary>
        /// <param name="floor">Floor number (0 indexed)</param>
        /// <param name="row">Row (0 indexed)</param>
        /// <param name="col">Column (0 indexed)</param>
        /// <returns>Return evacuation element with given coords, null if such coordinates are unmodelled</returns>
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

        /// <summary>
        /// Get evacuation element for base coordinate of given wall element position
        /// </summary>
        /// <param name="wep">Wall element position</param>
        /// <returns>Return evacuation element with given coords, null if such coordinates are unmodelled</returns>
        public EvacuationElement Get(WallElementPosition wep)
        {
            return Get(wep.Floor.Number, wep.Row, wep.Col);
        }

        /// <summary>
        /// Method initializes evacuation map from given building map
        /// </summary>
        /// <param name="bm">Building map</param>
        public void InitializeFromBuildingMap(BuildingMap bm)
        {
            _map = new SortedDictionary<int, IDictionary<int, IDictionary<int, EvacuationElement>>>();

            //setup shape and neighbourhood passages
            foreach(var e in bm.Floors)
            {
                Floor f = e.Value;
                int level = e.Key;
                
                IDictionary<int, IDictionary<int, EvacuationElement>> floorMap = new SortedDictionary<int, IDictionary<int, EvacuationElement>>();
                foreach (var row in f.Tiles)
                {
                    IDictionary<int, EvacuationElement> tempRow = new SortedDictionary<int, EvacuationElement>();
                    foreach (var tile in row.Value)
                    {
                        EvacuationElement ee = new EvacuationElement(tile.Value.Capacity);
                        //setup passages
                        for (int i = 0; i < 4; ++i)
                        {
                            ee.NeighboursPassages[i] = tile.Value.Side[i];
                        }

                        tempRow.Add(tile.Key, ee);
                    }
                    floorMap.Add(row.Key, tempRow);
                }
                _map.Add(level, floorMap);
            }

            //setup neighbourhood
            foreach (var floor in _map)
            {
                foreach (var row in floor.Value)
                {
                    foreach (var tile in row.Value)
                    {
                        tile.Value.Neighbours[(int)Direction.DOWN] = Get(floor.Key, row.Key + 1, tile.Key);
                        tile.Value.Neighbours[(int)Direction.UP] = Get(floor.Key, row.Key - 1, tile.Key);
                        tile.Value.Neighbours[(int)Direction.LEFT] = Get(floor.Key, row.Key, tile.Key - 1);
                        tile.Value.Neighbours[(int)Direction.RIGHT] = Get(floor.Key, row.Key, tile.Key + 1);

                        //creating special evacuation element for stairs
                        for (int i = 0; i < 4; ++i)
                        {
                            if (tile.Value.NeighboursPassages[i].Type == WallElementType.STAIR_ENTRY)
                            {
                                tile.Value.Neighbours[i] = new StairsEvacuationElement((StairsEntry)tile.Value.NeighboursPassages[i], this);
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Methos places given people group in evacuation map
        /// </summary>
        /// <param name="group">People group</param>
        public void SetPeopleGroup(PeopleGroup group)
        {
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
                        element.Value.Setup(0);
                        element.Value.ExistsPathToExit = false;
                    }
        }

        /// <summary>
        /// Initaliziation NextStep and Passage properties (evacuation routes in fact) from given fenotype based on neighbourhood
        /// </summary>
        /// <param name="fenotype">Given fenotype</param>
        public void MapFenotype(List<List<Direction>> fenotype)
        {
            Exits = new List<EvacuationElement>();
            var floorsEnumerator = fenotype.GetEnumerator();
            List<Direction>.Enumerator floorSegmentsEnumerator;

            foreach(var floorMap in _map)
            {
                //not enough floors in fenotype
                if (!floorsEnumerator.MoveNext())
                {
                    throw new BadFenotypeLengthException();
                }

                floorSegmentsEnumerator = floorsEnumerator.Current.GetEnumerator();

                foreach(var row in floorMap.Value)
                {
                    foreach(var tile in row.Value)
                    {
                        EvacuationElement element = tile.Value;

                        //not enough tiles in floor fenotype
                        if (!floorSegmentsEnumerator.MoveNext())
                        {
                            throw new BadFenotypeLengthException();
                        }

                        Direction direction = floorSegmentsEnumerator.Current;

                        element.Passage = element.NeighboursPassages[(int)direction];
                        if (element.Passage.CanPassThrough)
                        {
                            element.NextStep = element.Neighbours[(int)direction];
                            if (element.NextStep == null)
                                Exits.Add(element);
                        }
                        else
                        {
                            throw new UnexpectedWallException();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Based on NextStep in reverse order (from exits) creates list
        /// of people groups that can be evacuated.
        /// </summary>
        internal IEnumerable<EvacuationElement> GetPossibleEvacuationGroups()
        {
            return Exits.SelectMany(x => x.GetPossibleEvaucationGroups());
        }
    }
}

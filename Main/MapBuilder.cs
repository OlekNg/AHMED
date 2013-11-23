using Common.DataModel;
using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Main
{
    /// <summary>
    /// Builds building map and people map specific for simulator.
    /// </summary>
    public class MapBuilder
    {
        /// <summary>
        /// Source data to create building and people simulator maps.
        /// </summary>
        private Building _building;

        public MapBuilder(Building building)
        {
            _building = building;
        }

        public Structure.BuildingMap BuildBuildingMap()
        {
            Structure.BuildingMap result = new Structure.BuildingMap();

            // Source floors
            List<Floor> floors = _building.Floors.OrderBy(x => x.Level).ToList();

            // Floors, walls and doors.
            foreach (var f in floors)
            {
                int width = f.Segments[0].Count;
                int height = f.Segments.Count;

                Structure.Floor floor = new Structure.Floor();
                floor.Setup((uint)width, (uint)height, 3);

                for (int row = 0; row < f.Segments.Count; row++)
                {
                    for (int col = 0; col < f.Segments[row].Count; col++)
                    {
                        var segment = f.Segments[row][col];
                        if (segment.Type != SegmentType.FLOOR) continue;

                        floor.SetFloor((uint)row, (uint)col, (uint)segment.Capacity);

                        var sides = segment.GetSideElements();

                        foreach (Side s in typeof(Side).GetEnumValues())
                        {
                            var sideElement = sides[s];
                            int r = (s == Side.BOTTOM) ? row + 1 : row;
                            int c = (s == Side.RIGHT) ? col + 1 : col;
                            Structure.WallPlace place = (s == Side.TOP || s == Side.BOTTOM) ? Structure.WallPlace.TOP : Structure.WallPlace.LEFT;

                            if (sideElement.Type == SideElementType.WALL)
                                floor.SetWall((uint)r, (uint)c, place);

                            if (sideElement.Type == SideElementType.DOOR)
                                floor.SetDoor((uint)r, (uint)c, (uint)sideElement.Capacity, place);
                        }
                    }
                }

                result.AddFloor(floor);
            }

            // Stairs
            foreach (var stairsPair in _building.Stairs)
            {
                Structure.Stairs stairs = new Structure.Stairs((uint)stairsPair.First.Capacity, (uint)stairsPair.First.Delay);
                // TODO
                result.AddStairs(stairs);
            }

            return result;
        }

        public Structure.PeopleMap BuildPeopleMap()
        {
            Structure.PeopleMap result = new Structure.PeopleMap();

            return result;
        }
    }
}

using Common.DataModel;
using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Main.ViewModel
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
                floor.Setup(width, height, 3);

                for (int row = 0; row < f.Segments.Count; row++)
                {
                    for (int col = 0; col < f.Segments[row].Count; col++)
                    {
                        var segment = f.Segments[row][col];
                        if (segment.Type != SegmentType.FLOOR) continue;

                        floor.SetFloor(row, col, segment.Capacity);

                        var sides = segment.GetSideElements();

                        foreach (Direction s in typeof(Direction).GetEnumValues())
                        {
                            var sideElement = sides[s];
                            int r = (s == Direction.DOWN) ? row + 1 : row;
                            int c = (s == Direction.RIGHT) ? col + 1 : col;
                            Structure.WallPlace place = (s == Direction.UP || s == Direction.DOWN) ? Structure.WallPlace.TOP : Structure.WallPlace.LEFT;

                            if (sideElement.Type == SideElementType.WALL)
                                floor.SetWall(r, c, place);

                            if (sideElement.Type == SideElementType.DOOR)
                                floor.SetDoor(r, c, sideElement.Capacity, place);
                        }
                    }
                }

                result.AddFloor(floor);
            }

            // Stairs
            foreach (var stairsPair in _building.Stairs)
            {
                Structure.Stairs stairs = new Structure.Stairs(stairsPair.First.Capacity, stairsPair.First.Delay);

                // row, col, floor (level)
                int r, c, f;
                Structure.WallPlace pos;

                // First stairs entry.
                var first = stairsPair.First;
                f = first.Level;
                r = first.Orientation == Direction.DOWN ? first.Row + 1 : first.Row;
                c = first.Orientation == Direction.RIGHT ? first.Col + 1 : first.Col;
                pos = (first.Orientation == Direction.LEFT || first.Orientation == Direction.RIGHT) ? Structure.WallPlace.LEFT : Structure.WallPlace.TOP;

                Structure.WallElementPosition wep1 = new Structure.WallElementPosition(f, r, c, pos);
                Structure.StairsEntry se1 = new Structure.StairsEntry(first.EntranceCapacity, wep1);

                // Second stairs entry.
                var second = stairsPair.Second;
                f = second.Level;
                r = second.Orientation == Direction.DOWN ? second.Row + 1 : second.Row;
                c = second.Orientation == Direction.RIGHT ? second.Col + 1 : second.Col;
                pos = (second.Orientation == Direction.LEFT || second.Orientation == Direction.RIGHT) ? Structure.WallPlace.LEFT : Structure.WallPlace.TOP;

                Structure.WallElementPosition wep2 = new Structure.WallElementPosition(f, r, c, pos);
                Structure.StairsEntry se2 = new Structure.StairsEntry(second.EntranceCapacity, wep2);

                stairs.SetEntries(se1, se2);

                // Add to result.
                result.AddStairs(stairs);
            }

            return result;
        }

        public Structure.PeopleMap BuildPeopleMap()
        {
            Structure.PeopleMap result = new Structure.PeopleMap();

            foreach (var f in _building.Floors)
            {
                for (int row = 0; row < f.Segments.Count; row++)
                {
                    for (int col = 0; col < f.Segments[row].Count; col++)
                    {
                        var segment = f.Segments[row][col];
                        if (segment.PeopleCount > 0)
                        {
                            Structure.PeopleGroup group = new Structure.PeopleGroup(f.Level, row, col, segment.PeopleCount);
                            result.People.Add(group);
                        }

                    }
                }
            }

            return result;
        }
    }
}

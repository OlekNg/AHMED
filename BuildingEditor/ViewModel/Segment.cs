using Common.DataModel.Enums;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BuildingEditor.ViewModel
{
    [ImplementPropertyChanged]
    public class Segment
    {
        protected List<SideElement> _outerWalls = new List<SideElement>();
        public Floor Floor { get; protected set; }

        public Segment(Floor owner)
        {
            Floor = owner;

            LeftSide = new SideElement();
            TopSide = new SideElement();
            RightSide = new SideElement();
            BottomSide = new SideElement();

            TopLeftCorner = new SideElement();
            TopRightCorner = new SideElement();
            BottomRightCorner = new SideElement();
            BottomLeftCorner = new SideElement();

            // Default type and capacity.
            Type = SegmentType.FLOOR;
            Capacity = 10;

            FlowValue = Int32.MaxValue;
        }

        public Segment(Floor owner, Common.DataModel.Segment segment)
            : this(owner)
        {
            Type = segment.Type;
            Orientation = segment.Orientation;
            Capacity = segment.Capacity;
            PeopleCount = segment.PeopleCount;

            LeftSide = new SideElement(segment.LeftSide);
            TopSide = new SideElement(segment.TopSide);
            RightSide = new SideElement(segment.RightSide);
            BottomSide = new SideElement(segment.BottomSide);
        }

        public Segment(Floor owner, Segment segment)
            : this(owner)
        {
            Capacity = segment.Capacity;
            PeopleCount = segment.PeopleCount;
            Type = segment.Type == SegmentType.NONE ? SegmentType.NONE : SegmentType.FLOOR;

            LeftSide = new SideElement(segment.LeftSide);
            TopSide = new SideElement(segment.TopSide);
            RightSide = new SideElement(segment.RightSide);
            BottomSide = new SideElement(segment.BottomSide);
        }

        #region Properties
        public int Row { get; set; }
        public int Column { get; set; }
        public int Level { get { return Floor.Level; } }

        public Room Room { get; set; }

        public Direction Fenotype { get; set; }
        public string GenotypeText
        {
            get
            {
                switch (Fenotype)
                {
                    case Direction.LEFT:
                        return "00";
                    case Direction.UP:
                        return "10";
                    case Direction.RIGHT:
                        return "11";
                    case Direction.DOWN:
                        return "01";
                }

                return "";
            }
        }

        public bool Solution { get; set; }

        public bool Preview { get; set; }
        public SegmentType PreviewType { get; set; }
        public Direction PreviewOrientation { get; set; }

        public object AdditionalData { get; set; }
        public string StairsInfo
        {
            get
            {
                StairsPair sp = AdditionalData as StairsPair;
                if (sp == null) return "";
                var stairs = this == sp.First.AssignedSegment ? sp.Second : sp.First;

                return String.Format("{0}:{1}:{2}", stairs.EntranceCapacity, stairs.Capacity, stairs.Delay);
            }
        }

        public int FlowValue { get; set; }

        public int Capacity { get; set; }
        public int PeopleCount { get; set; }
        public SegmentType Type { get; set; }
        public Direction Orientation { get; set; }

        public Segment LeftSegment { get; set; }
        public Segment TopSegment { get; set; }
        public Segment RightSegment { get; set; }
        public Segment BottomSegment { get; set; }

        public SideElement LeftSide { get; set; }
        public SideElement TopSide { get; set; }
        public SideElement RightSide { get; set; }
        public SideElement BottomSide { get; set; }

        public SideElement TopLeftCorner { get; set; }
        public SideElement TopRightCorner { get; set; }
        public SideElement BottomRightCorner { get; set; }
        public SideElement BottomLeftCorner { get; set; }
        #endregion

        /// <summary>
        /// Checks for available directions from given segment.
        /// It excludes directions that lead to walls.
        /// Does not excludes directions that might lead to another small adjacent loop.
        /// (it allows that change - change is good for genetic algorithm).
        /// </summary>
        /// <param name="segment">Considered segment.</param>
        /// <returns>List of available directions.</returns>
        public List<Direction> GetAvailableDirections()
        {
            // From segment of type NONE there is no available directions.
            if (Type == SegmentType.NONE)
                return new List<Direction>();

            List<Direction> result = new List<Direction>() { Direction.LEFT, Direction.UP, Direction.RIGHT, Direction.DOWN };

            var sideElements = GetSideElements();
            var neighbours = GetNeighbours();

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
        public List<Direction> GetAvailableDirections(Direction except)
        {
            List<Direction> result = GetAvailableDirections();
            result.Remove(except);
            return result;
        }

        public Segment GetNeighbour(Direction side)
        {
            Segment result = null;

            switch (side)
            {
                case Direction.LEFT:
                    result = LeftSegment; break;
                case Direction.UP:
                    result = TopSegment; break;
                case Direction.RIGHT:
                    result = RightSegment; break;
                case Direction.DOWN:
                    result = BottomSegment; break;
            }

            return result;
        }

        /// <summary>
        /// Returns segment which this segment leads to (determined by fenotype).
        /// </summary>
        /// <returns></returns>
        public Segment GetNextSegment()
        {
            if (Type == SegmentType.STAIRS)
            {
                // Get segment that is on exit of second stairs from pair.
                StairsPair pair = (StairsPair)AdditionalData;
                Segment secondStairs;
                if (this == pair.First.AssignedSegment)
                    secondStairs = pair.Second.AssignedSegment;
                else
                    secondStairs = pair.First.AssignedSegment;
                return secondStairs.GetNeighbour(secondStairs.Orientation);
            }
            else
                return GetNeighbour(Fenotype);
        }

        public Dictionary<Direction, Segment> GetNeighbours()
        {
            Dictionary<Direction, Segment> result = new Dictionary<Direction, Segment>();

            result.Add(Direction.LEFT, LeftSegment);
            result.Add(Direction.UP, TopSegment);
            result.Add(Direction.RIGHT, RightSegment);
            result.Add(Direction.DOWN, BottomSegment);

            return result;
        }

        public SideElement GetSideElement(Direction side)
        {
            SideElement result = null;

            switch (side)
            {
                case Direction.LEFT:
                    result = LeftSide; break;
                case Direction.UP:
                    result = TopSide; break;
                case Direction.RIGHT:
                    result = RightSide; break;
                case Direction.DOWN:
                    result = BottomSide; break;
            }

            return result;
        }

        public Dictionary<Direction, SideElement> GetSideElements()
        {
            Dictionary<Direction, SideElement> result = new Dictionary<Direction, SideElement>();

            result.Add(Direction.LEFT, LeftSide);
            result.Add(Direction.UP, TopSide);
            result.Add(Direction.RIGHT, RightSide);
            result.Add(Direction.DOWN, BottomSide);

            return result;
        }

        public void SetSide(Direction side, SideElementType value)
        {
            GetSideElement(side).Type = value;
        }

        public void Flood(int value, Room room)
        {
            // Stop recursion.
            if (FlowValue <= value || Type == SegmentType.NONE)
                return;

            // If stairs - then move to segment at exit of second stairs in pair.
            if (Type == SegmentType.STAIRS)
            {
                GetNextSegment().Flood(value, null);
                return;
            }

            // Update segment flow value.
            FlowValue = value;
            if (Room == null)
            {
                (room ?? new Room()).AddSegment(this);
            }

            // Flood available directions.
            var directions = GetAvailableDirections();
            foreach (var dir in directions)
            {
                var next = GetNeighbour(dir);
                if (next != null)
                {
                    next.Flood(value + 1, GetSideElement(dir).Type == SideElementType.DOOR ? null : Room);
                }
            }
        }

        /// <summary>
        /// Sets proper type for corners to render correctly (walls without gaps).
        /// TODO: Optimize it (too much structural and repetitive code).
        /// </summary>
        public void UpdateCorners()
        {
            // Top left corner.
            if (LeftSide.Type == SideElementType.WALL ||
                TopSide.Type == SideElementType.WALL ||
                (LeftSegment != null && LeftSegment.TopSide.Type == SideElementType.WALL) ||
                (TopSegment != null && TopSegment.LeftSide.Type == SideElementType.WALL))
                TopLeftCorner.Type = SideElementType.WALL;
            else
                TopLeftCorner.Type = SideElementType.NONE;

            // Top Right corner.
            if (RightSide.Type == SideElementType.WALL ||
                TopSide.Type == SideElementType.WALL ||
                (RightSegment != null && RightSegment.TopSide.Type == SideElementType.WALL) ||
                (TopSegment != null && TopSegment.RightSide.Type == SideElementType.WALL))
                TopRightCorner.Type = SideElementType.WALL;
            else
                TopRightCorner.Type = SideElementType.NONE;

            // Bottom Right corner.
            if (RightSide.Type == SideElementType.WALL ||
                BottomSide.Type == SideElementType.WALL ||
                (RightSegment != null && RightSegment.BottomSide.Type == SideElementType.WALL) ||
                (BottomSegment != null && BottomSegment.RightSide.Type == SideElementType.WALL))
                BottomRightCorner.Type = SideElementType.WALL;
            else
                BottomRightCorner.Type = SideElementType.NONE;

            // Bottom left corner.
            if (LeftSide.Type == SideElementType.WALL ||
                BottomSide.Type == SideElementType.WALL ||
                (LeftSegment != null && LeftSegment.BottomSide.Type == SideElementType.WALL) ||
                (BottomSegment != null && BottomSegment.LeftSide.Type == SideElementType.WALL))
                BottomLeftCorner.Type = SideElementType.WALL;
            else
                BottomLeftCorner.Type = SideElementType.NONE;
        }

        /// <summary>
        /// Sets wall if adjacent segment is of type NONE (for each side).
        /// </summary>
        public void UpdateOuterWalls()
        {
            // Clear old outer walls to prevent them becoming as placed internal walls.
            _outerWalls.ForEach(x => { if (x.Type == SideElementType.WALL) x.Type = SideElementType.NONE; });
            _outerWalls.Clear();

            // Clear walls if we are type none.
            if (Type == SegmentType.NONE)
            {
                foreach (Direction s in typeof(Direction).GetEnumValues())
                {
                    Segment segment = GetNeighbour(s);
                    if (segment == null || segment.Type == SegmentType.NONE)
                        SetSide(s, SideElementType.NONE);
                }
                return;
            }

            // Set walls around stairs except entry
            if (Type == SegmentType.STAIRS)
            {
                foreach (Direction s in typeof(Direction).GetEnumValues())
                    if (s != Orientation)
                        SetSide(s, SideElementType.WALL);

                return;
            }

            // Set walls and add them to current outer walls list.
            foreach (Direction s in typeof(Direction).GetEnumValues())
            {
                // We don't want to overwrite doors.
                if (GetSideElement(s).Type == SideElementType.DOOR) continue;

                Segment segment = GetNeighbour(s);
                if (segment == null || segment.Type == SegmentType.NONE)
                {
                    SetSide(s, SideElementType.WALL);
                    _outerWalls.Add(GetSideElement(s));
                }
            }
        }

        public Common.DataModel.Segment ToDataModel()
        {
            Common.DataModel.Segment result = new Common.DataModel.Segment();

            result.Capacity = Capacity;
            result.Type = Type;
            result.Orientation = Orientation;
            result.PeopleCount = PeopleCount;

            result.LeftSide = LeftSide.ToDataModel();
            result.TopSide = TopSide.ToDataModel();
            result.RightSide = RightSide.ToDataModel();
            result.BottomSide = BottomSide.ToDataModel();

            return result;
        }
    }
}

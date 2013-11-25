using Common.DataModel.Enums;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BuildingEditor.Logic
{
    [ImplementPropertyChanged]
    public class Segment
    {
        protected List<SideElement> _outerWalls;

        public Segment()
        {
            _outerWalls = new List<SideElement>();

            LeftSide = new SideElement();
            TopSide = new SideElement();
            RightSide = new SideElement();
            BottomSide = new SideElement();

            TopLeftCorner = new SideElement();
            TopRightCorner = new SideElement();
            BottomRightCorner = new SideElement();
            BottomLeftCorner = new SideElement();

            Type = SegmentType.FLOOR;
        }

        public Segment(SegmentType type = SegmentType.FLOOR)
            : this()
        {
            Type = type;
            Capacity = 5;
        }

        public Segment(Common.DataModel.Segment segment)
            : this()
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

        #region Properties
        public int Row { get; set; }
        public int Column { get; set; }

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

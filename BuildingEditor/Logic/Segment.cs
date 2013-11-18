using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WPFTest.Logic
{
    public enum SegmentType { FLOOR, STAIRS, NONE }

    [ImplementPropertyChanged]
    public class Segment
    {
        protected List<SideElement> _outerWalls;

        public Segment(SegmentType type = SegmentType.FLOOR)
        {
            _outerWalls = new List<SideElement>();

            Type = type;
            Capacity = 5;

            LeftSide = new SideElement();
            TopSide = new SideElement();
            RightSide = new SideElement();
            BottomSide = new SideElement();

            TopLeftCorner = new SideElement();
            TopRightCorner = new SideElement();
            BottomRightCorner = new SideElement();
            BottomLeftCorner = new SideElement();
        }

        #region Properties
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Preview { get; set; }

        public int Capacity { get; set; }
        public int PeopleCount { get; set; }
        public SegmentType Type { get; set; }

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

        public Segment GetNeighbour(Side side)
        {
            Segment result = null;

            switch (side)
            {
                case Side.LEFT:
                    result = LeftSegment; break;
                case Side.TOP:
                    result = TopSegment; break;
                case Side.RIGHT:
                    result = RightSegment; break;
                case Side.BOTTOM:
                    result = BottomSegment; break;
            }

            return result;
        }

        public SideElement GetSideElement(Side side)
        {
            SideElement result = null;

            switch (side)
            {
                case Side.LEFT:
                    result = LeftSide; break;
                case Side.TOP:
                    result = TopSide; break;
                case Side.RIGHT:
                    result = RightSide; break;
                case Side.BOTTOM:
                    result = BottomSide; break;
            }

            return result;
        }

        public void SetSide(Side side, SideElementType value)
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
                foreach (Side s in typeof(Side).GetEnumValues())
                {
                    Segment segment = GetNeighbour(s);
                    if (segment == null || segment.Type == SegmentType.NONE)
                        SetSide(s, SideElementType.NONE);
                }
                return;
            }

            // Set walls and add them to current outer walls list.
            foreach (Side s in typeof(Side).GetEnumValues())
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
    }
}

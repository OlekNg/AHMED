using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFTest.Logic
{
    public enum SegmentType { FLOOR, NONE }

    [ImplementPropertyChanged]
    public class Segment
    {
        public Segment(SegmentType type = SegmentType.FLOOR)
        {
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

        public int Capacity { get; set; }
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

        public void UpdateCorners()
        {
            // Top left corner.
            if(LeftSide.Type == SideElementType.WALL ||
                TopSide.Type == SideElementType.WALL ||
                (LeftSegment != null && LeftSegment.TopSide.Type == SideElementType.WALL) ||
                (TopSegment != null && TopSegment.LeftSide.Type == SideElementType.WALL))
                TopLeftCorner.Type = SideElementType.WALL;
            else
                TopLeftCorner.Type = SideElementType.NONE;

            // Top Right corner.
            if(RightSide.Type == SideElementType.WALL ||
                TopSide.Type == SideElementType.WALL ||
                (RightSegment != null && RightSegment.TopSide.Type == SideElementType.WALL) ||
                (TopSegment != null && TopSegment.RightSide.Type == SideElementType.WALL))
                TopRightCorner.Type = SideElementType.WALL;
            else
                TopRightCorner.Type = SideElementType.NONE;

            // Bottom Right corner.
            if(RightSide.Type == SideElementType.WALL ||
                BottomSide.Type == SideElementType.WALL ||
                (RightSegment != null && RightSegment.BottomSide.Type == SideElementType.WALL) ||
                (BottomSegment != null && BottomSegment.RightSide.Type == SideElementType.WALL))
                BottomRightCorner.Type = SideElementType.WALL;
            else
                BottomRightCorner.Type = SideElementType.NONE;

            // Bottom left corner.
            if(LeftSide.Type == SideElementType.WALL ||
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
            // Clear walls if we are type none.
            if (Type == SegmentType.NONE)
            {
                if (LeftSegment == null || LeftSegment.Type == SegmentType.NONE)
                    LeftSide.Type = SideElementType.NONE;

                if (TopSegment == null || TopSegment.Type == SegmentType.NONE)
                    TopSide.Type = SideElementType.NONE;

                if (RightSegment == null || RightSegment.Type == SegmentType.NONE)
                    RightSide.Type = SideElementType.NONE;

                if (BottomSegment == null || BottomSegment.Type == SegmentType.NONE)
                    BottomSide.Type = SideElementType.NONE;
                return;
            }

            // Set walls.
            if (LeftSegment == null || LeftSegment.Type == SegmentType.NONE)
                LeftSide.Type = SideElementType.WALL;

            if (TopSegment == null || TopSegment.Type == SegmentType.NONE)
                TopSide.Type = SideElementType.WALL;

            if (RightSegment == null || RightSegment.Type == SegmentType.NONE)
                RightSide.Type = SideElementType.WALL;

            if (BottomSegment == null || BottomSegment.Type == SegmentType.NONE)
                BottomSide.Type = SideElementType.WALL;
        }
    }
}

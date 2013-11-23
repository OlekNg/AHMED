using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingEditor.DataModel
{
    [Serializable]
    public class Segment
    {
        public BuildingEditor.Logic.SegmentType Type;
        public BuildingEditor.Logic.Side Orientation;
        public int Capacity;
        public int PeopleCount;

        public SideElement LeftSide;
        public SideElement TopSide;
        public SideElement RightSide;
        public SideElement BottomSide;

        public Segment() { }

        public Segment(Logic.Segment segment)
        {
            Capacity = segment.Capacity;
            Type = segment.Type;
            Orientation = segment.Orientation;
            PeopleCount = segment.PeopleCount;

            LeftSide = new SideElement(segment.LeftSide);
            TopSide = new SideElement(segment.TopSide);
            RightSide = new SideElement(segment.RightSide);
            BottomSide = new SideElement(segment.BottomSide);
        }
    }
}

using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DataModel
{
    [Serializable]
    public class Segment
    {
        public SegmentType Type;
        public Side Orientation;
        public int Capacity;
        public int PeopleCount;

        public SideElement LeftSide;
        public SideElement TopSide;
        public SideElement RightSide;
        public SideElement BottomSide;

        public Segment() { }
    }
}

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

        public Dictionary<Side, SideElement> GetSideElements()
        {
            Dictionary<Side, SideElement> result = new Dictionary<Side, SideElement>();

            result.Add(Side.LEFT, LeftSide);
            result.Add(Side.TOP, TopSide);
            result.Add(Side.RIGHT, RightSide);
            result.Add(Side.BOTTOM, BottomSide);

            return result;
        }
    }
}

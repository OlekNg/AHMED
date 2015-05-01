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
        public Direction Orientation;
        public int Capacity;
        public int PeopleCount;
        public bool Danger;

        public SideElement LeftSide;
        public SideElement TopSide;
        public SideElement RightSide;
        public SideElement BottomSide;

        public Segment() { }

        public Dictionary<Direction, SideElement> GetSideElements()
        {
            Dictionary<Direction, SideElement> result = new Dictionary<Direction, SideElement>();

            result.Add(Direction.LEFT, LeftSide);
            result.Add(Direction.UP, TopSide);
            result.Add(Direction.RIGHT, RightSide);
            result.Add(Direction.DOWN, BottomSide);

            return result;
        }
    }
}

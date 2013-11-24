using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataModel.Enums
{
    public enum Direction { LEFT, UP, RIGHT, DOWN }
    public enum Corner { TOP_LEFT = 1, TOP_RIGHT = 2, BOTTOM_RIGHT = 3, BOTTOM_LEFT = 4 }

    // SEGMENT
    public enum SegmentType { FLOOR, STAIRS, NONE }

    public enum SideElementType { NONE, WALL, DOOR }
}

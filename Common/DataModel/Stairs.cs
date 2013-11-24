using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DataModel
{
    [Serializable]
    public class Stairs
    {
        public int Capacity;
        public int Delay;
        public int Row;
        public int Col;
        public int Level;
        public Direction Orientation;

        public Stairs() { }
    }
}

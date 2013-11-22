using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingEditor.DataModel
{
    [Serializable]
    public class Stairs
    {
        public int Capacity;
        public int Delay;
        public int Row;
        public int Col;
        public int Level;

        public Stairs() { }

        public Stairs(Logic.Stairs stairs)
        {
            Capacity = stairs.Capacity;
            Delay = stairs.Delay;
            Level = stairs.Level;
            Row = stairs.AssignedSegment.Row;
            Col = stairs.AssignedSegment.Column;
        }
    }
}

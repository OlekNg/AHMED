using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingEditor.DataModel
{
    [Serializable]
    public class StairsPair
    {
        public Stairs First;
        public Stairs Second;

        public StairsPair() { }

        public StairsPair(Logic.StairsPair stairsPair)
        {
            First = new Stairs(stairsPair.First);
            Second = new Stairs(stairsPair.Second);
        }
    }
}

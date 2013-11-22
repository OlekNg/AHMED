using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingEditor.Logic
{
    [ImplementPropertyChanged]
    public class Stairs
    {
        public int Level { get; set; }
        public int Capacity { get; set; }
        public int Delay { get; set; }

        public Segment AssignedSegment { get; set; }
    }

    [ImplementPropertyChanged]
    public class StairsPair
    {
        public Stairs First { get; set; }
        public Stairs Second { get; set; }

        public string Text
        {
            get
            {
                return String.Format("{0} - {1}", First.Level, Second.Level);
            }
        }
    }
}

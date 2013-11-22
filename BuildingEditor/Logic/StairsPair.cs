using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<StairsPair> _stairs;

        public StairsPair(ObservableCollection<StairsPair> stairs)
        {
            _stairs = stairs;
        }

        public Stairs First { get; set; }
        public Stairs Second { get; set; }

        public string Text
        {
            get
            {
                return String.Format("{0} - {1}", First.Level, Second.Level);
            }
        }

        public void SetAdditionalData()
        {
            First.AssignedSegment.AdditionalData = this;
            Second.AssignedSegment.AdditionalData = this;
        }

        public void Destroy()
        {
            First.AssignedSegment.Type = SegmentType.FLOOR;
            Second.AssignedSegment.Type = SegmentType.FLOOR;
            _stairs.Remove(this);
        }
    }
}

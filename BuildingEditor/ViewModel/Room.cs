using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingEditor.ViewModel
{
    /// <summary>
    /// Class that groups segments into room.
    /// </summary>
    public class Room
    {
        public int Id { get; set; }
        public int NumberOfDoors { get; set; }
        public List<Segment> Segments { get; set; }

        public Room()
        {
            Segments = new List<Segment>();
        }

        public void AddSegment(Segment segment)
        {
            if (segment.Room != null)
                throw new InvalidOperationException("Segment has already assigned room.");

            Segments.Add(segment);
            segment.Room = this;
        }
    }
}

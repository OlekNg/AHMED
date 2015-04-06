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
        public List<Segment> Segments { get; set; }

        public int Id { get; protected set; }

        private static int _counter = 1;

        public static void ResetCounter()
        {
            _counter = 1;
        }

        public Room()
        {
            Id = _counter++;
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

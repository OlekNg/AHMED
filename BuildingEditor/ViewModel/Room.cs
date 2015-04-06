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

        /// <summary>
        /// Simple segment flow value based evacuation algorithm (especially for single-door rooms).
        /// </summary>
        public void ApplySimpleEvacuation()
        {
            Segments.ForEach(segment =>
            {
                var dirs = segment.GetAvailableDirections();
                var bestDirection = dirs.OrderBy(x => segment.GetNeighbour(x).FlowValue)
                    .First();

                segment.Fenotype = bestDirection;
            });
        }
    }
}

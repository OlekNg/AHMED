using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingEditor.DataModel
{
    [Serializable]
    public class Floor
    {
        public List<List<Segment>> Segments;
        public int Level;

        public Floor()
        {
            Segments = new List<List<Segment>>();
        }

        public Floor(BuildingEditor.Logic.Floor floor)
            : this()
        {
            Level = floor.Level;

            for (int row = 0; row < floor.Data.Count; row++)
            {
                List<Segment> segmentRow = new List<Segment>();
                for (int col = 0; col < floor.Data[row].Count; col++)
                {
                    segmentRow.Add(new Segment(floor.Data[row][col]));
                }
                Segments.Add(segmentRow);
            }
        }
    }
}

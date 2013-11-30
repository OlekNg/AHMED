using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DataModel
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
    }
}

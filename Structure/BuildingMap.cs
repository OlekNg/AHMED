using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class BuildingMap
    {
        public uint Height { get; set; }

        public uint Width { get; set; }

        public Door[] Doors { get; set; }

        public Wall[] Walls { get; set; }

        public FloorSquare[][] Floor { get; set; }
    }
}

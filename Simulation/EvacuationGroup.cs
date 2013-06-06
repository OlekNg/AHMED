using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class EvacuationGroup
    {
        public uint Quantity { get; set; }

        public uint Ticks { get; set; }

        public bool Processed { get; set; }

        public EvacuationElement Position { get; set; }
    }
}

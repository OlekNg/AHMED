using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class EscapedGroup
    {
        public uint Quantity { get; set; }

        public uint Ticks { get; set; }

        public EscapedGroup(EvacuationElement ee)
        {
            Quantity = ee.PeopleQuantity;
            Ticks = ee.Ticks;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;

namespace Simulation
{
    public class EvacuationElement
    {
        public FloorSquare FloorSquare { get; set; }

        public IWallElement Passage { get; set; }

        public uint PeopleQuantity { get; set; }

        public uint Ticks { get; set; }

        public bool Processed { get; set; }

        public EvacuationElement NextStep { get; set; }

        public uint PeopleQuantityLeft { 
            get 
            { 
                return FloorSquare.Capacity - PeopleQuantity; 
            } 
        }
    }
}

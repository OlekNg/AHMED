using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class PeopleGroup
    {
        public uint X { get; set; }

        public uint Y { get; set; }

        public uint Quantity { get; set; }

        public PeopleGroup(uint x, uint y, uint quantity)
        {
            X = x;
            Y = y;
            Quantity = quantity;
        }
    }
}

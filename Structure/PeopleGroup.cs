using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class PeopleGroup
    {
        public uint Row { get; set; }

        public uint Col { get; set; }

        public uint Quantity { get; set; }

        public PeopleGroup(uint row, uint col, uint quantity)
        {
            Row = row;
            Col = col;
            Quantity = quantity;
        }
    }
}

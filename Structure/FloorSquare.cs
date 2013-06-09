using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class FloorSquare
    {
        public uint Capacity { get; set; }

        public IWallElement[] Side { get; set; }

        public FloorSquare(uint c)
        {
            Capacity = c;
            Side = new IWallElement[4];
        }

        public IWallElement GetSide(Direction dir)
        {
            return Side[(int)dir];
        }
    }
}

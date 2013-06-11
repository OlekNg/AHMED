using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class Door : IWallElement
    {
        public bool CanPassThrough
        {
            get
            {
                return true;
            }
        }

        public uint Capacity { get; set; }

        public bool Draw { get; set; }

        public Door(uint c)
        {
            Capacity = c;
            Draw = true;
        }

        public Door(uint c, bool d)
        {
            Capacity = c;
            Draw = d;
        }
    }
}

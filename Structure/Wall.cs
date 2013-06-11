using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class Wall : IWallElement
    {
        public bool CanPassThrough
        {
            get
            {
                return false;
            }
        }

        public uint Capacity
        {
            get
            {
                return 0;
            }
        }

        public bool Draw
        {
            get
            {
                return true;
            }
        }
    }
}

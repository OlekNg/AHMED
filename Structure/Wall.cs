using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class Wall : IWallElement
    {
        public bool CanPassThrough()
        {
            return false;
        }

        public int Capacity() {
            return 0;
        }
    }
}

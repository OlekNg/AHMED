using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class Door : IWallElement
    {
        private uint _capacity;

        public bool CanPassThrough()
        {
            return true;
        }

        public uint Capacity()
        {
            return _capacity;
        }
    }
}

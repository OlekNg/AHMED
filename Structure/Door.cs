using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class Door : IWallElement
    {
        private int _capacity;

        public bool CanPassThrough()
        {
            return true;
        }

        public int Capacity()
        {
            return _capacity;
        }
    }
}

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
    }
}

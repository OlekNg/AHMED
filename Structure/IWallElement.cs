using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public interface IWallElement
    {
        bool CanPassThrough();

        public uint Capacity();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structure
{
    public class StandardPassage : IWallElement
    {
        public bool CanPassThrough { get { return true; } }

        public int Efficiency { get; private set; }

        public bool Draw { get { return false; } }

        public WallElementType Type { get { return WallElementType.STANDARD_PASSAGE; } }

        public StandardPassage(int efficiency)
        {
            Efficiency = efficiency;
        }
    }
}

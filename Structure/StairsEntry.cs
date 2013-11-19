using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class StairsEntry : IWallElement
    {
        private uint _efficiency;

        public bool CanPassThrough { get { return true; } }

        public uint Efficiency { get { return _efficiency; } }

        public bool Draw { get { return true; } }

        public WallElementType Type { get { return WallElementType.STAIR_ENTRY; } }

        private Stairs _stairs;

        public Stairs ConnectedStairs { get { return _stairs; } }

        private int _id;

        public int ID { get { return _id; } }

        public WallElementPosition Position { get; set; }

        public StairsEntry(uint e, WallElementPosition wep)
        {
            _efficiency = e;
            Position = wep;
        }

        public void BindStairs(Stairs s, int id)
        {
            _stairs = s;
            _id = id;
        }
    }
}

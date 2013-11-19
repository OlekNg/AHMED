using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class Stairs
    {
        private uint _capacity;

        public uint Capacity { get { return _capacity; } }

        private uint _delay;

        public uint Delay { get { return _delay; } }

        private StairsEntry[] _entries;

        public StairsEntry[] Entries { get { return _entries; } }

        public Stairs(uint c, uint d)
        {
            _entries = new StairsEntry[2];
            _capacity = c;
            _delay = d;
        }

        public void SetEntries(StairsEntry se1, StairsEntry se2)
        {
            _entries[0] = se1;
            _entries[1] = se2;
            se1.BindStairs(this, 0);
            se2.BindStairs(this, 1);
            
        }

        public StairsEntry GetEntry(int id)
        {
            return _entries[id];
        }
    }
}

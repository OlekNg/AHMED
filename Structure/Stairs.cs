using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    /// <summary>
    /// Class used for describing stairs
    /// </summary>
    public class Stairs
    {
        /// <summary>
        /// Total capacity for this stairs
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Starting delay for each people group
        /// </summary>
        public int Delay { get; private set; }

        /// <summary>
        /// Both entries for that stairs
        /// </summary>
        public StairsEntry[] Entries { get; private set; }

        /// <summary>
        /// Simple constructor initializes all properties
        /// </summary>
        /// <param name="c">Capacity</param>
        /// <param name="d">Delay</param>
        public Stairs(int c, int d)
        {
            Entries = new StairsEntry[2];
            Capacity = c;
            Delay = d;
        }

        /// <summary>
        /// Set both entries (first with index 0, second - with index 1)
        /// </summary>
        /// <param name="se1">0 indexed entry</param>
        /// <param name="se2">1 indexed entry</param>
        public void SetEntries(StairsEntry se1, StairsEntry se2)
        {
            Entries[0] = se1;
            Entries[1] = se2;
            se1.BindStairs(this, 0);
            se2.BindStairs(this, 1);
            
        }

        /// <summary>
        /// Get entry with given index
        /// </summary>
        /// <param name="id">Index</param>
        /// <returns>Proper entry</returns>
        public StairsEntry GetEntry(int id)
        {
            return Entries[id];
        }
    }
}

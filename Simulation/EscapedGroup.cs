using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    /// <summary>
    /// Simple class to describe group of people which managed to escape
    /// </summary>
    public class EscapedGroup
    {
        /// <summary>
        /// People count
        /// </summary>
        public uint Quantity { get; set; }

        /// <summary>
        /// How many ticks took escape?
        /// </summary>
        public uint Ticks { get; set; }

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="q">People quantity</param>
        /// <param name="t">Ticks count</param>
        public EscapedGroup(uint q, uint t)
        {
            Quantity = q;
            Ticks = t;
        }
    }
}

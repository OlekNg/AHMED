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
        public int Quantity { get; set; }

        /// <summary>
        /// How many ticks took escape?
        /// </summary>
        public int Ticks { get; set; }

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="q">People quantity</param>
        /// <param name="t">Ticks count</param>
        public EscapedGroup(int q, int t)
        {
            Quantity = q;
            Ticks = t;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structure
{
    /// <summary>
    /// Class used for standard passage - working like doors, but it is not draw in visualization
    /// </summary>
    public class StandardPassage : IWallElement
    {
        /// <summary>
        /// You can pass the standard passage
        /// </summary>
        public bool CanPassThrough { get { return true; } }

        /// <summary>
        /// Standard efficiency
        /// </summary>
        public int Efficiency { get; private set; }

        /// <summary>
        /// Do not draw standard passage
        /// </summary>
        public bool Draw { get { return false; } }

        /// <summary>
        /// Type of std passage
        /// </summary>
        public WallElementType Type { get { return WallElementType.STANDARD_PASSAGE; } }

        /// <summary>
        /// Position of std passage
        /// </summary>
        public WallElementPosition Position { get; set; }

        /// <summary>
        /// Initialize efficiency
        /// </summary>
        /// <param name="efficiency">Given efficiency</param>
        public StandardPassage(int efficiency)
        {
            Efficiency = efficiency;
        }
    }
}

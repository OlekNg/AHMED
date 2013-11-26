using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    /// <summary>
    /// Class for wall
    /// </summary>
    public class Wall : IWallElement
    {
        /// <summary>
        /// There is no way to pass through wall
        /// </summary>
        public bool CanPassThrough
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// No one can pass through the wall
        /// </summary>
        public int Efficiency
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Wall are alwyas drawn
        /// </summary>
        public bool Draw
        {
            get
            {
                return true;
            }
        }

        public WallElementType Type 
        {
            get
            {
                return WallElementType.WALL;
            }
        }
    }
}

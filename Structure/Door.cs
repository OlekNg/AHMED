using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    /// <summary>
    /// Door class
    /// </summary>
    public class Door : IWallElement
    {
        /// <summary>
        /// you can pass the door
        /// </summary>
        public bool CanPassThrough
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Door efficiency
        /// </summary>
        public int Efficiency { get; set; }

        /// <summary>
        /// Is there need to draw this door in visualiser
        /// </summary>
        public bool Draw { get { return true; } }

        /// <summary>
        /// Initalize door with given efficiency
        /// </summary>
        /// <param name="c">Efficiency</param>
        public Door(int c)
        {
            Efficiency = c;
        }

        public WallElementType Type
        {
            get
            {
                return WallElementType.DOOR;
            }
        }

        public WallElementPosition Position { get; set; }
    }
}

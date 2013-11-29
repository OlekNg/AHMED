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
        public bool Draw { get; set; }

        /// <summary>
        /// Initalize door with given efficiency
        /// </summary>
        /// <param name="c">Efficiency</param>
        public Door(int c)
        {
            Efficiency = c;
            Draw = true;
        }

        /// <summary>
        /// Initalize door with given efficiency and draw mode
        /// </summary>
        /// <param name="c">Efficiency</param>
        /// <param name="d">To draw or not to draw?</param>
        public Door(int c, bool d)
        {
            Efficiency = c;
            Draw = d;
        }

        public WallElementType Type
        {
            get
            {
                return WallElementType.DOOR;
            }
        }
    }
}

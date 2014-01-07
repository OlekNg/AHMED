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
        /// You can pass the door
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
        /// Draw door
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

        /// <summary>
        /// Door type
        /// </summary>
        public WallElementType Type
        {
            get
            {
                return WallElementType.DOOR;
            }
        }

        /// <summary>
        /// Door position
        /// </summary>
        public WallElementPosition Position { get; set; }
    }
}

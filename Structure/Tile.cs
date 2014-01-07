using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    /// <summary>
    /// Class describing one floor square
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Capacity of the floor tile
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Wall elements sorrounding this floor tile
        /// </summary>
        public IWallElement[] Side { get; set; }

        //public TilePosition Position { get; internal set; }

        /// <summary>
        /// Initalize floor square with given capacity
        /// </summary>
        /// <param name="c">Capacity</param>
        public Tile(int c)
        {
            Capacity = c;
            Side = new IWallElement[4];
        }

        /// <summary>
        /// Return wall element in given direction
        /// </summary>
        /// <param name="dir">Direction</param>
        /// <returns>Wall element</returns>
        public IWallElement GetSide(Direction dir)
        {
            return Side[(int)dir];
        }

        /// <summary>
        /// Set wall element in given direction
        /// </summary>
        /// <param name="dir">Direction</param>
        /// <param name="side">Wall element to set</param>
        public void SetSide(Direction dir, IWallElement side)
        {
            Side[(int)dir] = side;
        }
    }
}

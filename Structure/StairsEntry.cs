using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    /// <summary>
    /// Class used for stairs entry modelling
    /// </summary>
    public class StairsEntry : IWallElement
    {
        /// <summary>
        /// You can pass the stairs entry
        /// </summary>
        public bool CanPassThrough { get { return true; } }

        /// <summary>
        /// Stairs entry efficiency
        /// </summary>
        public int Efficiency { get; private set; }

        /// <summary>
        /// Draw stairs entry
        /// </summary>
        public bool Draw { get { return true; } }

        /// <summary>
        /// Stairs entry type
        /// </summary>
        public WallElementType Type { get { return WallElementType.STAIR_ENTRY; } }

        /// <summary>
        /// Stairs bound with this entry
        /// </summary>
        public Stairs ConnectedStairs { get; private set; }

        /// <summary>
        /// Index of this entry in bounded stairs
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Stairs entry position
        /// </summary>
        public WallElementPosition Position { get; set; }

        /// <summary>
        /// Simple constructor - initializes efficiency
        /// </summary>
        /// <param name="e">Efficiency</param>
        public StairsEntry(int e)
        {
            Efficiency = e;
        }

        /// <summary>
        /// Make connection between given stairs and this entry with given index
        /// </summary>
        /// <param name="s">Stairs</param>
        /// <param name="id">Index in stairs of this entry</param>
        public void BindStairs(Stairs s, int id)
        {
            ConnectedStairs = s;
            ID = id;
        }
    }
}

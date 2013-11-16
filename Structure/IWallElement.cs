using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public enum WallElementType { WALL, DOOR, STAIR_ENTRY }

    /// <summary>
    /// Interface desctibing each wall element
    /// </summary>
    public interface IWallElement
    {
        /// <summary>
        /// Can you pass through this wall element
        /// </summary>
        bool CanPassThrough { get; }

        /// <summary>
        /// Wall element efficiency (measured in number of people which can go through this wall element)
        /// </summary>
        uint Efficiency { get; }

        /// <summary>
        /// Should this element be drawn in visualiser
        /// </summary>
        bool Draw { get; }

        WallElementType Type { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structure
{
    /// <summary>
    /// Class positiong tile in floor
    /// </summary>
    public class TilePosition
    {
        /// <summary>
        /// Row
        /// </summary>
        public int Row { get; private set; }

        /// <summary>
        /// Column
        /// </summary>
        public int Col { get; private set; }

        /// <summary>
        /// Initialize row and column
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        public TilePosition(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structure
{
    public class TilePosition
    {
        public int Row { get; private set; }

        public int Col { get; private set; }

        public TilePosition(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}

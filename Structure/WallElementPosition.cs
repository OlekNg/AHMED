using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class WallElementPosition
    {
        private int _floor;

        private int _row;

        private int _col;

        private WallPlace _place;

        public int Floor { get { return _floor; } }

        public int Row { get { return _row; } }

        public int Col { get { return _col; } }

        public WallPlace Place { get { return _place; } }

        public WallElementPosition(int f, int r, int c, WallPlace wp)
        {
            _floor = f;
            _row = r;
            _col = c;
            _place = wp;
        }

        public WallElementPosition FindAdjacentSquare()
        {
            if (_place == WallPlace.TOP)
            {
                return new WallElementPosition(_floor, _row - 1, _col, _place);
            }
            else
            {
                return new WallElementPosition(_floor, _row, _col - 1, _place);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class WallElementPosition
    {
        private uint _floor;

        private uint _row;

        private uint _col;

        private WallPlace _place;

        public uint Floor { get { return _floor; } }

        public uint Row { get { return _row; } }

        public uint Col { get { return _col; } }

        public WallPlace Place { get { return _place; } }

        public WallElementPosition(uint f, uint r, uint c, WallPlace wp)
        {
            _floor = f;
            _row = r;
            _col = c;
            _place = wp;
        }

        public WallElementPosition FindAdjacentSquare()
        {
            if (_place == WallPlace.LEFT)
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

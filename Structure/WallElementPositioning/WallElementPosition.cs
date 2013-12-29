using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataModel.Enums;

namespace Structure
{
    public abstract class WallElementPosition
    {
        public Floor Floor { get; private set; }

        public int Row { get; private set; }

        public int Col { get; private set; }

        public abstract Direction Orientation { get; }

        public static WallElementPosition Create(Floor f, int row, int col, Direction dir)
        {
            switch (dir)
            {
                case Direction.DOWN:
                    return new WallElementDownPosition(f, row, col);
                case Direction.LEFT:
                    return new WallElementLeftPosition(f, row, col);
                case Direction.RIGHT:
                    return new WallElementRightPosition(f, row, col);
                default:
                    return new WallElementUpPosition(f, row, col);
            }                
        }

        public WallElementPosition(Floor f, int r, int c)
        {
            Floor = f;
            Row = r;
            Col = c;
        }

        public TilePosition GetTilePosition()
        {
            return new TilePosition(Row, Col);
        }

        public abstract WallElementPosition GetAdjacentPosition();
    }




    public class WallElementRightPosition : WallElementPosition
    {
        public override Direction Orientation
        {
            get
            {
                return Direction.RIGHT;
            }
        }

        public WallElementRightPosition(Floor f, int row, int col) 
            : base(f, row, col)
        {
            
        }

        public override WallElementPosition GetAdjacentPosition()
        {
            return new WallElementLeftPosition(Floor, Row, Col + 1);
        }
    }




    public class WallElementLeftPosition : WallElementPosition
    {
        public override Direction Orientation
        {
            get
            {
                return Direction.LEFT;
            }
        }

        public WallElementLeftPosition(Floor f, int row, int col)
            : base(f, row, col)
        {

        }

        public override WallElementPosition GetAdjacentPosition()
        {
            return new WallElementRightPosition(Floor, Row, Col - 1);
        }
    }




    public class WallElementUpPosition : WallElementPosition
    {
        public override Direction Orientation
        {
            get
            {
                return Direction.UP;
            }
        }

        public WallElementUpPosition(Floor f, int row, int col)
            : base(f, row, col)
        {

        }

        public override WallElementPosition GetAdjacentPosition()
        {
            return new WallElementDownPosition(Floor, Row - 1, Col);
        }
    }




    public class WallElementDownPosition : WallElementPosition
    {
        public override Direction Orientation
        {
            get
            {
                return Direction.DOWN;
            }
        }

        public WallElementDownPosition(Floor f, int row, int col)
            : base(f, row, col)
        {

        }

        public override WallElementPosition GetAdjacentPosition()
        {
            return new WallElementUpPosition(Floor, Row + 1, Col);
        }
    }


}

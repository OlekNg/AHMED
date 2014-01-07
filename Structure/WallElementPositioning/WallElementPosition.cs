using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataModel.Enums;

namespace Structure
{
    /// <summary>
    /// Abstract class used for positiong all wall elements
    /// </summary>
    public abstract class WallElementPosition
    {
        /// <summary>
        /// Base coordinate floor
        /// </summary>
        public Floor Floor { get; private set; }

        /// <summary>
        /// Base coordinate row
        /// </summary>
        public int Row { get; private set; }

        /// <summary>
        /// Base coordinate col
        /// </summary>
        public int Col { get; private set; }

        /// <summary>
        /// Orientation of wall element 
        /// </summary>
        public abstract Direction Orientation { get; }

        /// <summary>
        /// Method factory
        /// </summary>
        /// <param name="f">Floor</param>
        /// <param name="row">Row</param>
        /// <param name="col">Col</param>
        /// <param name="dir">Orientation</param>
        /// <returns>Created position</returns>
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

        /// <summary>
        /// Constructor, used for creating base coordinates
        /// </summary>
        /// <param name="f">Floor</param>
        /// <param name="r">Row</param>
        /// <param name="c">Col</param>
        public WallElementPosition(Floor f, int r, int c)
        {
            Floor = f;
            Row = r;
            Col = c;
        }

        /// <summary>
        /// Converter to tile position
        /// </summary>
        /// <returns>Tile position</returns>
        public TilePosition GetTilePosition()
        {
            return new TilePosition(Row, Col);
        }

        /// <summary>
        /// Create WallElementPosition for the same wal element, based on adjoining tile
        /// </summary>
        /// <returns></returns>
        public abstract WallElementPosition GetAdjacentPosition();
    }

    /// <summary>
    /// Class used for "right" positioniong
    /// </summary>
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

    /// <summary>
    /// Class used for "left" positioning
    /// </summary>
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

    /// <summary>
    /// Class used for "up" positioning
    /// </summary>
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

    /// <summary>
    /// Class used for "down" positioning
    /// </summary>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public enum WallPosition { TOP, LEFT }

    public class BuildingMap
    {
        public uint Height { get; set; }

        public uint Width { get; set; }

        public uint StandardPassageEfficency { get; set; }

        public FloorSquare[][] Floor { get; set; }

        public void Setup(uint w, uint h, uint standardPassageEfficency)
        {
            Height = h;
            Width = w;
            StandardPassageEfficency = standardPassageEfficency;
            Floor = new FloorSquare[h][];
            for (int i = 0; i < h; ++i)
            {
                Floor[i] = new FloorSquare[w];
            }
        }

        public void SetFloor(uint row, uint col, uint capacity)
        {
            Floor[row][col] = new FloorSquare(capacity);
            for(int i = 0; i < 4; ++i)
            {
                Floor[row][col].Side[i] = new Door(StandardPassageEfficency, false);
            }
        }

        public void SetWall(uint row, uint col, WallPosition position)
        {
            SetWallElement(row, col, new Wall(), position);
        }

        public void SetDoor(uint row, uint col, uint capacity, WallPosition position)
        {
            SetWallElement(row, col, new Door(capacity), position);
        }

        private void SetWallElement(uint row, uint col, IWallElement wallElement, WallPosition wallPosition)
        {
            if (wallPosition == WallPosition.LEFT)
            {
                if (col != 0)
                {
                    //not first column
                    //set as right side of adjacent floor tile
                    Floor[row][col - 1].SetSide(Direction.RIGHT, wallElement);
                }
                if (col != Width)
                {
                    //not last column
                    //set as left side
                    Floor[row][col].SetSide(Direction.LEFT, wallElement);
                }
            }
            else
            {
                if (row != 0)
                {
                    //not first row
                    //set as bootom side of upper tile
                    Floor[row - 1][col].SetSide(Direction.DOWN, wallElement);
                }
                if (row != Height)
                {
                    //not last row
                    //set as top side
                    Floor[row][col].SetSide(Direction.UP, wallElement);
                }
            }
        }
    }
}

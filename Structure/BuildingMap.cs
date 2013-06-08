using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genetics;

namespace Structure
{
    public enum WallPosition { TOP, LEFT }

    public class BuildingMap
    {
        public uint Height { get; set; }

        public uint Width { get; set; }

        //public Door[] Doors { get; set; }

        //public Wall[] Walls { get; set; }

        public FloorSquare[][] Floor { get; set; }

        public void SetSize(uint w, uint h)
        {
            Height = h;
            Width = w;
            Floor = new FloorSquare[h][];
            for (int i = 0; i < h; ++i)
            {
                Floor[i] = new FloorSquare[w];
            }
        }

        public void SetFloor(uint row, uint col, uint capacity)
        {
            Floor[row][col] = new FloorSquare(capacity);
        }

        public void SetWall(uint row, uint col, WallPosition position)
        {
            SetWallElement(row, col, new Wall(), position);
        }

        public void SetDoor(uint row, uint col, uint capacity, WallPosition position)
        {
            SetWallElement(row, col, new Door { Capacity = capacity }, position);
        }

        private void SetWallElement(uint row, uint col, IWallElement wallElement, WallPosition wallPosition)
        {
            if (wallPosition == WallPosition.LEFT)
            {
                if (col != 0)
                {
                    //not first column
                    //set as right side of adjacent floor tile
                    Floor[row][col - 1].Side[(int)Chromosome.Allele.RIGHT] = wallElement;
                }
                if (col != Width)
                {
                    //not last column
                    //set as left side
                    Floor[row][col].Side[(int)Chromosome.Allele.LEFT] = wallElement;
                }
            }
            else
            {
                if (row != 0)
                {
                    //not first row
                    //set as bootom side of upper tile
                    Floor[row - 1][col].Side[(int)Chromosome.Allele.DOWN] = wallElement;
                }
                if (row != Height)
                {
                    //not last row
                    //set as top side
                    Floor[row][col].Side[(int)Chromosome.Allele.UP] = wallElement;
                }
            }
        }
    }
}

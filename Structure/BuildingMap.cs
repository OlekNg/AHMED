using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class BuildingMap
    {
        public uint Height { get; set; }

        public uint Width { get; set; }

        //public Door[] Doors { get; set; }

        //public Wall[] Walls { get; set; }

        public FloorSquare[][] Floor { get; set; }

        public void SetSize(uint h, uint w)
        {
            Height = h;
            Width = w;
            Floor = new FloorSquare[w][];
            for (int i = 0; i < h; ++i)
            {
                Floor[i] = new FloorSquare[h];
            }
        }

        public void SetFloor(uint x, uint y, uint capacity)
        {
            Floor[x][y] = new FloorSquare(capacity);
        }

        public void SetWall(uint x, uint y)
        {
            SetWallElement(x, y, new Wall());
        }

        public void SetDoor(uint x, uint y, uint capacity)
        {
            SetWallElement(x, y, new Door { Capacity = capacity });
        }

        private void SetWallElement(uint x, uint y, IWallElement wallElement)
        {
            if (x % 2 == 0)
            {
                //column walls
                if (x == 0)
                {
                    //first column
                    
                }
                else
                {

                }
            }
            else
            {
                //row walls

            }
        }
    }
}

using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class Floor
    {
        public int Height { get; set; }

        public int Width { get; set; }

        public int PassageEfficiency { get; set; }

        public FloorSquare[][] Base { get; set; }

        /// <summary>
        /// Initialize floor map with given size and efficiency
        /// </summary>
        /// <param name="w">Floor width</param>
        /// <param name="h">Floor height</param>
        /// <param name="standardPassageEfficency">Standard efficiency for not set passages</param>
        public void Setup(int w, int h, int standardPassageEfficency)
        {
            Height = h;
            Width = w;
            PassageEfficiency = standardPassageEfficency;
            Base = new FloorSquare[h][];
            for (int i = 0; i < h; ++i)
            {
                Base[i] = new FloorSquare[w];
            }
        }

        /// <summary>
        /// Set floor with given coordinates and capacity
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="capacity">Capacity</param>
        public void SetFloor(int row, int col, int capacity)
        {
            Base[row][col] = new FloorSquare(capacity);
            for (int i = 0; i < 4; ++i)
            {
                Base[row][col].Side[i] = new StandardPassage(PassageEfficiency);
            }
        }

        /// <summary>
        /// Set wall oriented to floor square with given coordinates
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="position">Wall orientation</param>
        public void SetWall(int row, int col, WallPlace position)
        {
            SetWallElement(row, col, new Wall(), position);
        }

        /// <summary>
        /// Set door with given efficiency oriented to floor square with given coordinates
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="capacity">Door eficiency</param>
        /// <param name="position">Door orientation</param>
        public void SetDoor(int row, int col, int capacity, WallPlace position)
        {
            SetWallElement(row, col, new Door(capacity), position);
        }

        public void SetStairsEntry(int row, int col, WallPlace position, StairsEntry se)
        {
            SetWallElement(row, col, se, position);
        }

        /// <summary>
        /// Set given wall element oritented to floor with given coordinations
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="wallElement">Wall element</param>
        /// <param name="wallPosition">Wall element orientation</param>
        private void SetWallElement(int row, int col, IWallElement wallElement, WallPlace wallPosition)
        {
            if (wallPosition == WallPlace.LEFT)
            {
                if (col != 0)
                {
                    //not first column
                    //set as right side of adjacent floor tile
                    if (Base[row][col - 1] != null)
                        Base[row][col - 1].SetSide(Direction.RIGHT, wallElement);
                }
                if (col != Width)
                {
                    //not last column
                    //set as left side
                    if (Base[row][col] != null)
                        Base[row][col].SetSide(Direction.LEFT, wallElement);
                }
            }
            else
            {
                if (row != 0)
                {
                    //not first row
                    //set as bootom side of upper tile
                    if (Base[row - 1][col] != null)
                        Base[row - 1][col].SetSide(Direction.DOWN, wallElement);
                }
                if (row != Height)
                {
                    //not last row
                    //set as top side
                    if (Base[row][col] != null)
                        Base[row][col].SetSide(Direction.UP, wallElement);
                }
            }
        }
    }
}

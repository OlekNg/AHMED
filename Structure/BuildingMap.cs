using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    /// <summary>
    /// Enum containing possible locations of wall element (door, wall) next to each floor square
    /// </summary>
    public enum WallPosition { TOP, LEFT }

    /// <summary>
    /// Class containing informations about building structure
    /// </summary>
    public class BuildingMap
    {
        /// <summary>
        /// Height of the map
        /// </summary>
        public uint Height { get; set; }

        /// <summary>
        /// Width of the map
        /// </summary>
        public uint Width { get; set; }
        
        /// <summary>
        /// Efficiency for passages between floor squares (when there is no door set)
        /// </summary>
        public uint StandardPassageEfficency { get; set; }

        /// <summary>
        /// Floor (null when there is no floor set with coordinates)
        /// </summary>
        public FloorSquare[][] Floor { get; set; }

        /// <summary>
        /// Initialize building map with given size and efficiency
        /// </summary>
        /// <param name="w">Map width</param>
        /// <param name="h">Map height</param>
        /// <param name="standardPassageEfficency">Standard efficiency for not set passages</param>
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

        /// <summary>
        /// Set floor with given coordinates and capacity
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="capacity">Capacity</param>
        public void SetFloor(uint row, uint col, uint capacity)
        {
            Floor[row][col] = new FloorSquare(capacity);
            for(int i = 0; i < 4; ++i)
            {
                Floor[row][col].Side[i] = new Door(StandardPassageEfficency, false);
            }
        }

        /// <summary>
        /// Set wall oriented to floor square with given coordinates
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="position">Wall orientation</param>
        public void SetWall(uint row, uint col, WallPosition position)
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
        public void SetDoor(uint row, uint col, uint capacity, WallPosition position)
        {
            SetWallElement(row, col, new Door(capacity), position);
        }

        /// <summary>
        /// Set given wall element oritented to floor eith given coordinations
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="wallElement">Wall element</param>
        /// <param name="wallPosition">Wall element orientation</param>
        private void SetWallElement(uint row, uint col, IWallElement wallElement, WallPosition wallPosition)
        {
            if (wallPosition == WallPosition.LEFT)
            {
                if (col != 0)
                {
                    //not first column
                    //set as right side of adjacent floor tile
                    if(Floor[row][col - 1] != null)
                        Floor[row][col - 1].SetSide(Direction.RIGHT, wallElement);
                }
                if (col != Width)
                {
                    //not last column
                    //set as left side
                    if(Floor[row][col] != null)
                        Floor[row][col].SetSide(Direction.LEFT, wallElement);
                }
            }
            else
            {
                if (row != 0)
                {
                    //not first row
                    //set as bootom side of upper tile
                    if (Floor[row - 1][col] != null)
                        Floor[row - 1][col].SetSide(Direction.DOWN, wallElement);
                }
                if (row != Height)
                {
                    //not last row
                    //set as top side
                    if (Floor[row][col] != null)
                        Floor[row][col].SetSide(Direction.UP, wallElement);
                }
            }
        }
    }
}

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
        //public int Height { get; set; }

        //public int Width { get; set; }

        public int PassageEfficiency { get; set; }

        public IDictionary<int, IDictionary<int, Tile>> Tiles;

        public int Number { get; internal set; }

        public int TilesCount { get; private set; }

        public Floor(int passageEfficiency)
        {
            PassageEfficiency = passageEfficiency;
            TilesCount = 0;
            Tiles = new SortedDictionary<int, IDictionary<int, Tile>>();
        }

        /// <summary>
        /// Set floor with given coordinates and capacity
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="capacity">Capacity</param>
        public void SetTile(int row, int col, int capacity)
        {
            Tile t = new Tile(capacity);
            t.Position = new TilePosition(row, col);
            for (int i = 0; i < 4; ++i)
            {
                t.Side[i] = new StandardPassage(PassageEfficiency);
            }

            //Tiles[row][col] = t;
            SetTile(row, col, t);

            //Base[row][col] = t;
            
        }

        public void SetTile(int row, int col, Tile t)
        {
            IDictionary<int, Tile> rowDict;
            if (!Tiles.TryGetValue(row, out rowDict))
            {
                rowDict = new SortedDictionary<int, Tile>();
                Tiles[row] = rowDict;
            }
            rowDict[col] = t;
            ++TilesCount;
        }


        public void UnsetFloor(int row, int col)
        {
            //TODO: dokonczyc
            Tile t = Get(row, col);
            if (t != null)
            {
                Tiles[row].Remove(col);
                t.Position = null;
                --TilesCount;
            }
        }

        /// <summary>
        /// Set wall oriented to floor square with given coordinates
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="position">Wall orientation</param>
        /*public void SetWall(int row, int col, WallPlace position)
        {
            SetWallElement(row, col, new Wall(), position);
        }*/

        /// <summary>
        /// Set door with given efficiency oriented to floor square with given coordinates
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="capacity">Door eficiency</param>
        /// <param name="position">Door orientation</param>
        /*public void SetDoor(int row, int col, int capacity, WallPlace position)
        {
            SetWallElement(row, col, new Door(capacity), position);
        }

        public void SetStairsEntry(int row, int col, WallPlace position, StairsEntry se)
        {
            SetWallElement(row, col, se, position);
        }*/

        public bool SetWallElement(int row, int col, Direction dir, IWallElement element)
        {
            WallElementPosition wep = WallElementPosition.Create(this, row, col, dir);

            if (PlaceWallElement(wep, element) || PlaceWallElement(wep.GetAdjacentPosition(), element))
            {
                element.Position = wep;
                return true;
            }
            
            return false;
        }

        private bool PlaceWallElement(WallElementPosition wep, IWallElement element)
        {
            Tile t = Get(wep.GetTilePosition());

            if (t != null)
            {
                t.SetSide(wep.Orientation, element);
                return true;
            }

            return false;
        }



        /// <summary>
        /// Set given wall element oritented to floor with given coordinations
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="wallElement">Wall element</param>
        /// <param name="wallPosition">Wall element orientation</param>
        /*private void SetWallElement(int row, int col, IWallElement wallElement, WallPlace wallPosition)
        {
            Tile t;
            if (wallPosition == WallPlace.LEFT)
            {
                if (col != 0)
                {
                    //not first column
                    //set as right side of adjacent floor tile
                    t = Get(row, col - 1);
                    if (t != null)
                        t.SetSide(Direction.RIGHT, wallElement);
                }
                if (col != Width)
                {
                    //not last column
                    //set as left side
                    t = Get(row, col);
                    if (t != null)
                        t.SetSide(Direction.LEFT, wallElement);
                }
            }
            else
            {
                if (row != 0)
                {
                    //not first row
                    //set as bootom side of upper tile
                    t = Get(row - 1, col);
                    if (t != null)
                        t.SetSide(Direction.DOWN, wallElement);
                }
                if (row != Height)
                {
                    //not last row
                    //set as top side
                    t = Get(row, col);
                    if (t != null)
                        t.SetSide(Direction.UP, wallElement);
                }
            }
        }*/









        public Tile Get(int row, int col)
        {
            IDictionary<int, Tile> rowDict;
            Tile result;
            if (!Tiles.TryGetValue(row, out rowDict))
                return null;

            if (!rowDict.TryGetValue(col, out result))
                result = null;

            return result;
        }

        public Tile Get(TilePosition tp)
        {
            return Get(tp.Row, tp.Col);
        }

        public Tile[] GetNeighbours(int x, int y)
        {
            Tile[] neighbours = new Tile[4];

            neighbours[(int)Direction.UP] = Get(x, y - 1);
            neighbours[(int)Direction.DOWN] = Get(x, y + 1);
            neighbours[(int)Direction.LEFT] = Get(x - 1, y);
            neighbours[(int)Direction.RIGHT] = Get(x + 1, y);

            return neighbours;
        }
    }
}

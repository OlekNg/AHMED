using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    /// <summary>
    /// Class for floor
    /// </summary>
    public class Floor
    {
        /// <summary>
        /// Standard passage efficiency
        /// </summary>
        public int PassageEfficiency { get; set; }

        /// <summary>
        /// Map with each modelled tile
        /// </summary>
        public IDictionary<int, IDictionary<int, Tile>> Tiles;

        /// <summary>
        /// Doors on the floor.
        /// </summary>
        public IList<Door> Doors;

        /// <summary>
        /// Number of floor
        /// </summary>
        public int Number { get; internal set; }

        /// <summary>
        /// Counter for modelled tiles
        /// </summary>
        public int TilesCount { get; private set; }

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="passageEfficiency">Standard passage efficiency</param>
        public Floor(int passageEfficiency)
        {
            PassageEfficiency = passageEfficiency;
            TilesCount = 0;
            Tiles = new SortedDictionary<int, IDictionary<int, Tile>>();
            Doors = new List<Door>();
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
            for (int i = 0; i < 4; ++i)
            {
                t.Side[i] = new StandardPassage(PassageEfficiency);
            }
            SetTile(row, col, t);            
        }

        /// <summary>
        /// Set floor tile with gicen coordinates
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="t">Floor tile</param>
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

        /// <summary>
        /// Removes tile from given coordinates
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        public void UnsetTile(int row, int col)
        {
            Tile t = Get(row, col);
            if (t != null)
            {
                Tiles[row].Remove(col);
                --TilesCount;
            }
        }

        /// <summary>
        /// Set given wall element with gicen coordinates and orientation
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="dir">Orienation</param>
        /// <param name="element">Wall element</param>
        /// <returns>True if there was at least one floor tile, adjoning this coordinates, false otherwise</returns>
        public bool SetWallElement(int row, int col, Direction dir, IWallElement element)
        {
            WallElementPosition wep = WallElementPosition.Create(this, row, col, dir);

            if (PlaceWallElement(wep, element) || PlaceWallElement(wep.GetAdjacentPosition(), element))
            {
                element.Position = wep;

                if (element.Type == WallElementType.DOOR)
                    Doors.Add((Door)element);
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Set given wall element with gicen coordinates and orientation
        /// </summary>
        /// <param name="wep">Wall place position</param>
        /// <param name="element">Wall element</param>
        /// <returns>True if there was at least one floor tile, adjoning this coordinates, false otherwise</returns>
        private bool PlaceWallElement(WallElementPosition wep, IWallElement element)
        {
            TilePosition tp = wep.GetTilePosition();
            Tile t = Get(tp.Row, tp.Col);

            if (t != null)
            {
                t.SetSide(wep.Orientation, element);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get tile with given coordinates
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <returns>Tile if there was modelled tile with that coords or null otherwise</returns>
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
    }
}

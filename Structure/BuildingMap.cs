using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    /// <summary>
    /// Class containing informations about building structure
    /// </summary>
    public class BuildingMap
    {
        public IDictionary<int, Floor> Floors { get; private set; }

        public IList<Stairs> Stairs { get; private set; }

        public BuildingMap(){
            Floors = new SortedDictionary<int, Floor>();
            Stairs = new List<Stairs>();
        }

        public void AddFloor(int number, Floor f)
        {
            Floors.Add(number, f);
            f.Number = number;
        }

        public void AddFloor(Floor f)
        {
            AddFloor(Floors.Count, f);
        }

        public void RemoveFloor(int id)
        {
            //TODO: dokonczyc
            Floors.Remove(id);
        }

        public void AddStairs(Stairs s)
        {
            //foreach (StairsEntry se in s.Entries)
            //{
            //
            //    Floors[se.Position.Floor.Number].SetStairsEntry(se.Position.Row, se.Position.Col, se.Position.Place, se);
            //Hm?}
            Stairs.Add(s);
        }

        public void RemoveStairs(int id)
        {
            Stairs.RemoveAt(id);
            //TODO: usunac powiazanie miedzy wejsciami a schodami
        }

        public Tile GetSquare(int floor, int row, int col)
        {
            //return Floors[floor].Base[row][col];
            Floor f;

            if(Floors.TryGetValue(floor, out f))
                return f.Get(row, col);

            return null;
        }
    }
}

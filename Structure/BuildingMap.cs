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
        /// <summary>
        /// Collection with all floors in building
        /// </summary>
        public IDictionary<int, Floor> Floors { get; private set; }

        /// <summary>
        /// Collection with all stairs in building
        /// </summary>
        public IList<Stairs> Stairs { get; private set; }

        /// <summary>
        /// Simple constructor
        /// </summary>
        public BuildingMap(){
            Floors = new SortedDictionary<int, Floor>();
            Stairs = new List<Stairs>();
        }

        /// <summary>
        /// Add floor with given number
        /// </summary>
        /// <param name="number">Number of the new floor</param>
        /// <param name="f">New floor</param>
        public void AddFloor(int number, Floor f)
        {
            Floors.Add(number, f);
            f.Number = number;
        }

        /// <summary>
        /// Add floor as last one
        /// </summary>
        /// <param name="f">New floor</param>
        public void AddFloor(Floor f)
        {
            AddFloor(Floors.Count, f);
        }

        /// <summary>
        /// Remove floor with given index
        /// </summary>
        /// <param name="id">Index</param>
        public void RemoveFloor(int id)
        {
            Floors.Remove(id);
        }

        /// <summary>
        /// Add stairs
        /// </summary>
        /// <param name="s">New stairs</param>
        public void AddStairs(Stairs s)
        {
            Stairs.Add(s);
        }

        /// <summary>
        /// Remove stairs with given index
        /// </summary>
        /// <param name="id">Index</param>
        public void RemoveStairs(int id)
        {
            Stairs.RemoveAt(id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics
{
    /// <summary>
    /// Result from simulator has to implement this interface.
    /// (This is expected data by genetic algorithm.)
    /// </summary>
    interface ISimulationResult
    {
        /// <summary>
        /// Avarage time that each persons needed
        /// to leave the building.
        /// </summary>
        public double AvgEscapeTime { get; }

        /// <summary>
        /// The longest escape time.
        /// </summary>
        public double MaxEscapeTime { get; }

        /// <summary>
        /// True if there are people left in the building.
        /// </summary>
        public bool PeopleLeftInBuilding { get; }
    }
}

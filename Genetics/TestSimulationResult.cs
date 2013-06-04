using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics
{
    /// <summary>
    /// Temporary test simulation result.
    /// </summary>
    class TestSimulationResult : ISimulationResult
    {
        private double _avgEscapeTime;
        private double _maxEscapeTime;
        private bool _peopleLeftInBuilding;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="avgTime">Avarage escape time.</param>
        /// <param name="maxTime">Max escape time.</param>
        /// <param name="peopleLeft">Is there people left in the building.</param>
        TestSimulationResult(double avgTime, double maxTime, bool peopleLeft)
        {
            _avgEscapeTime = avgTime;
            _maxEscapeTime = maxTime;
            _peopleLeftInBuilding = peopleLeft;
        }

        public double AvgEscapeTime
        {
            get { return _avgEscapeTime; }
        }

        public double MaxEscapeTime
        {
            get { return _maxEscapeTime; }
        }

        public bool PeopleLeftInBuilding
        {
            get { return _peopleLeftInBuilding; }
        }
    }
}

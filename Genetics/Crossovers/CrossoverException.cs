using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Crossovers
{
    /// <summary>
    /// May occur during crossing over.
    /// </summary>
    public class CrossoverException : Exception
    {
        public CrossoverException() : base() { }

        public CrossoverException(string message) : base(message) { }
    }
}

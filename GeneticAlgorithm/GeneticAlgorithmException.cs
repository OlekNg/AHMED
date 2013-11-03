using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics
{
    class GeneticAlgorithmException : Exception
    {
        public GeneticAlgorithmException() : base() { }

        public GeneticAlgorithmException(string message) : base(message) { }
    }
}

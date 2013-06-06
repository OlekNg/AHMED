using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Mutations
{
    /// <summary>
    /// May occur during crossing over.
    /// </summary>
    public class MutationException : Exception
    {
        public MutationException() : base() { }

        public MutationException(string message) : base(message) { }
    }
}

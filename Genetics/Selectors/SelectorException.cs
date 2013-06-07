using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Selectors
{
    public class SelectorException : Exception
    {
        public SelectorException() : base() { }

        public SelectorException(string message) : base(message) { }
    }
}

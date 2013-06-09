using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Repairers
{
    public class RepairerException  : Exception
    {
        public RepairerException() : base() { }

        public RepairerException(string message) : base(message) { }
    }
}

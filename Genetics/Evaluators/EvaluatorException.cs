using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Evaluators
{
    /// <summary>
    /// May occur during evaluating.
    /// </summary>
    public class EvaluatorException : Exception
    {
        public EvaluatorException() : base() { }

        public EvaluatorException(string message) : base(message) { }
    }
}

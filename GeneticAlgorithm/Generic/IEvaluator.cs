using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Generic
{
    public interface IEvaluator<T>
    {
        double Eval(T genotype);
    }
}

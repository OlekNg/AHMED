using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Generic
{
    public interface ICrossoverOperator<T>
    {
        Tuple<T, T> Crossover(T genotype1, T genotype2);
    }
}

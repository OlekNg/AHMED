using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Selectors
{
    /// <summary>
    /// Interface for selectors.
    /// </summary>
    public interface ISelector
    {
        List<Chromosome> Select(List<Chromosome> population);
    }
}

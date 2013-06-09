using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Repairers
{
    /// <summary>
    /// Interface for repairers.
    /// </summary>
    public interface IRepairer
    {
        /// <summary>
        /// Repairs chromosome and replaces original with fixed one.
        /// </summary>
        /// <param name="c">Chromosome to repair.</param>
        void RepairAndReplace(Chromosome c);

        /// <summary>
        /// Repairs chromosome not affecting original one.
        /// </summary>
        /// <param name="c">Chromosome to repair.</param>
        /// <returns>Fixed chromosome.</returns>
        Chromosome RepairAndCreate(Chromosome c);
    }
}

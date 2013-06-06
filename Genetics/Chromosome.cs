using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics
{
    public class Chromosome
    {
        /// <summary>
        /// Binary chain.
        /// </summary>
        private List<bool> _genotype;

        /// <summary>
        /// Default constructor - empty chromosome.
        /// </summary>
        public Chromosome()
        {
            _genotype = new List<bool>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="genotype">Initial genotype.</param>
        public Chromosome(bool[] genotype)
        {
            _genotype = new List<bool>(genotype);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="genotype">String that contains numbers 1 or 0.</param>
        public Chromosome(string genotype)
        {
            _genotype = new List<bool>(genotype.Length);

            for (int i = 0; i < genotype.Length; i++)
                _genotype.Add(genotype[i] == '1' ? true : false);
        }

        /// <summary>
        /// Property for binary chain.
        /// </summary>
        public List<bool> Genotype
        {
            get { return _genotype; }
            set { _genotype = value; }
        }

        /// <summary>
        /// Length of chromosome (its genotype).
        /// </summary>
        public int Length
        {
            get { return _genotype.Count; }
        }

        /// <summary>
        /// Performs crossover with other chromosome
        /// using specific number of crossover points.
        /// </summary>
        /// <param name="points">Number of points to split the chromosome into.</param>
        /// <param name="c">Second chromosome to crossover.</param>
        /// <returns>New chromosome.</returns>
        public Chromosome Crossover(uint points, Chromosome chromosome)
        {
            return null;
        }

        /// <summary>
        /// Mutates chromosome.
        /// </summary>
        /// <param name="probability">Probability of single bit mutation.</param>
        public void Mutate(double probability)
        {
            return;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(_genotype.Count);

            for (int i = 0; i < _genotype.Count; i++)
                sb.Append(_genotype[i] == true ? '1' : '0');

            return sb.ToString();
        }
    }
}

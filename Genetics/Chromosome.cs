using Genetics.Crossovers;
using Genetics.Mutations;
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
        /// Crossover operator used by Cross...() methods.
        /// </summary>
        public static ICrossoverOperator CrossoverOperator { get; set; }

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
        /// Mutation operator used by Mutate() methods.
        /// </summary>
        public static IMutationOperator MutationOperator { get; set; }

        /// <summary>
        /// Performs crossing over with chromosome c and creates new
        /// chromosomes. Doesn't affect original chromosomes.
        /// </summary>
        /// <param name="c">Second chromosome to crossover.</param>
        /// <returns>Recombined chromosomes.</returns>
        /// <exception cref="CrossoverException">Occurs while no crossover operator is defined.</exception>
        public Chromosome[] CrossAndCreate(Chromosome c)
        {
            if (CrossoverOperator == null)
                throw new CrossoverException("No crossover operator defined!");

            return CrossoverOperator.CrossAndCreate(this, c);
        }

        /// <summary>
        /// Performs crossing over with chromosome c and replaces genotype
        /// of original chromosomes.
        /// </summary>
        /// <param name="c">Second chromosome to crossover.</param>
        /// /// <exception cref="CrossoverException">Occurs while no crossover operator is defined.</exception>
        public void CrossAndReplace(Chromosome c)
        {
            if (CrossoverOperator == null)
                throw new CrossoverException("No crossover operator defined!");

            CrossoverOperator.CrossAndReplace(this, c);
        }

        /// <summary>
        /// Mutates chromosome with default probability.
        /// </summary>
        /// <exception cref="MutationException">Occurs while no mutation operator is defined.</exception>
        public void Mutate()
        {
            if (MutationOperator == null)
                throw new MutationException("No mutation operator defined!");

            MutationOperator.Mutate(this);
        }

        /// <summary>
        /// Mutates chromosome with given probability.
        /// </summary>
        /// <exception cref="MutationException">Occurs while no mutation operator is defined.</exception>
        public void Mutate(double probability)
        {
            if (MutationOperator == null)
                throw new MutationException("No mutation operator defined!");

            MutationOperator.Mutate(this, probability);
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

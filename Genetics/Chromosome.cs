using Genetics.Crossovers;
using Genetics.Mutations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genetics.Evaluators;

namespace Genetics
{
    public class Chromosome
    {
        /// <summary>
        /// Binary chain.
        /// </summary>
        private List<bool> _genotype;

        /// <summary>
        /// Chromosome value produced by evaluator.
        /// </summary>
        private double _value;

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

        public Chromosome(List<bool> genotype)
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
        /// Possible fenotype's alleles.
        /// </summary>
        public enum Allele { UP, DOWN, LEFT, RIGHT }

        /// <summary>
        /// Crossover operator used by Cross...() methods.
        /// </summary>
        public static ICrossoverOperator CrossoverOperator { get; set; }

        /// <summary>
        /// Evaluator used by Evaluate() method.
        /// </summary>
        public static IEvaluator Evaluator { get; set; }

        /// <summary>
        /// Chromosome's fenotype.
        /// </summary>
        public List<Allele> Fenotype
        {
            get
            {
                // To convert to fenotype bits have to be paired.
                if (Length % 2 != 0)
                    throw new Exception("Invalid chromosome! There should be even number of bits.");

                List<Allele> fenotype = new List<Allele>(Length / 2);
                for (int i = 0; i < Length; i += 2)
                {
                    if (_genotype[i] == true)
                    {
                        if (_genotype[i + 1] == true)
                            fenotype.Add(Allele.RIGHT); // 11
                        else
                            fenotype.Add(Allele.UP); // 10
                    }
                    else
                    {
                        if (_genotype[i + 1] == true)
                            fenotype.Add(Allele.DOWN); // 01
                        else
                            fenotype.Add(Allele.LEFT); // 00
                    }
                }

                return fenotype;
            }
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
        /// Mutation operator used by Mutate() methods.
        /// </summary>
        public static IMutationOperator MutationOperator { get; set; }

        /// <summary>
        /// Readonly accessor for chromosome's value.
        /// </summary>
        public double Value
        {
            get { return _value; }
        }

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
        /// Evaluates chromosome - stores evaluated value in the Value property.
        /// </summary>
        public void Evaluate()
        {
            if (Evaluator == null)
                throw new EvaluatorException("No evaluator defined!");

            _value = Evaluator.Evaluate(this);
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

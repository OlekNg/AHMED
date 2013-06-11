using Genetics.Crossovers;
using Genetics.Mutations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genetics.Evaluators;
using Structure;

namespace Genetics
{
    public class Chromosome : IComparable
    {
        /// <summary>
        /// Randomizer for creating random chromosome.
        /// </summary>
        private static Random _randomizer = new Random();

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

        public Chromosome(Chromosome c)
        {
            _genotype = new List<bool>(c._genotype);
            _value = c._value;
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
        public List<Direction> Fenotype
        {
            get
            {
                // To convert to fenotype bits have to be paired.
                if (Length % 2 != 0)
                    throw new Exception("Invalid chromosome! There should be even number of bits.");

                List<Direction> fenotype = new List<Direction>(Length / 2);
                for (int i = 0; i < Length; i += 2)
                {
                    if (_genotype[i] == true)
                    {
                        if (_genotype[i + 1] == true)
                            fenotype.Add(Direction.RIGHT); // 11
                        else
                            fenotype.Add(Direction.UP); // 10
                    }
                    else
                    {
                        if (_genotype[i + 1] == true)
                            fenotype.Add(Direction.DOWN); // 01
                        else
                            fenotype.Add(Direction.LEFT); // 00
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
        /// Creates random chromosome.
        /// </summary>
        /// <param name="length">Length of generated chromosome.</param>
        /// <returns>New chromosome.</returns>
        public static Chromosome CreateRandom(int length)
        {
            List<bool> genotype = new List<bool>(length);
            for (int i = 0; i < length; i++)
                genotype.Add(_randomizer.NextDouble() > 0.5 ? true : false);

            return new Chromosome(genotype);
        }

        /// <summary>
        /// Sorting by Value property.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            Chromosome c = (Chromosome)obj;

            if (c.Value > this.Value)
                return -1;
            else if (c.Value < this.Value)
                return 1;
            return 0;
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

        /// <summary>
        /// Changes fenotype allel (implicitly genotype).
        /// </summary>
        /// <param name="index">Index of fenotype element (allel).</param>
        /// <param name="dir">New value.</param>
        public void SetFenotype(int index, Direction dir)
        {
            int genotypeIndex = index * 2;

            // Out of range
            if (genotypeIndex > _genotype.Count || genotypeIndex < 0)
                return;

            switch (dir)
            {
                case Direction.UP: // 10
                    _genotype[genotypeIndex] = true;
                    _genotype[genotypeIndex + 1] = false;
                    break;

                case Direction.DOWN: // 01
                    _genotype[genotypeIndex] = false;
                    _genotype[genotypeIndex + 1] = true;
                    break;

                case Direction.LEFT: // 00
                    _genotype[genotypeIndex] = false;
                    _genotype[genotypeIndex + 1] = false;
                    break;

                case Direction.RIGHT: // 11
                    _genotype[genotypeIndex] = true;
                    _genotype[genotypeIndex + 1] = true;
                    break;

                default:
                    break;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(_genotype.Count);

            for (int i = 0; i < _genotype.Count; i++)
            {
                if (i % 14 == 0 && i != 0)
                    sb.Append("\n");
                else if (i % 2 == 0 && i != 0)
                    sb.Append(" ");

                sb.Append(_genotype[i] == true ? '1' : '0');
                
            }

            return sb.ToString();
        }

        
    }
}

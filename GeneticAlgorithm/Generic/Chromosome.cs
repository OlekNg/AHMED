using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Generic
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of genotype.</typeparam>
    public class Chromosome<T> : IChromosome
    {
        public Chromosome() { }
        public Chromosome(T genotype)
        {
            Genotype = genotype;
        }

        public static Func<T, IChromosome> CreateFunc { get; set; }
        public static ICrossoverOperator<T> CrossoverOperator { get; set; }
        public static IEvaluator<T> Evaluator { get; set; }
        public static IMutationOperator<T> MutationOperator { get; set; }
        public static IRepairer<T> Repairer { get; set; }

        public T Genotype { get; set; }
        public double Value { get; set; }

        public IChromosome Clone()
        {
            return CreateFunc(Genotype);
        }

        public void Crossover(IChromosome chromosome)
        {
            if (CrossoverOperator != null)
            {
                Chromosome<T> c = (Chromosome<T>)chromosome;
                var result = CrossoverOperator.Crossover(Genotype, c.Genotype);
                Genotype = result.Item1;
                c.Genotype = result.Item2;
            }
        }

        public void Eval()
        {
            if (Evaluator == null)
                throw new GeneticAlgorithmException("Evaluator not set. Evaluation cannot be performed.");

            Value = Evaluator.Eval(Genotype);
        }

        public void Mutate()
        {
            if (MutationOperator != null)
                MutationOperator.Mutate(Genotype);
        }

        public void Repair()
        {
            if (Repairer != null)
                Repairer.Repair(Genotype);
        }

        public int CompareTo(IChromosome other)
        {
            if (Value == other.Value)
                return 0;

            return Value > other.Value ? 1 : -1;
        }
    }
}

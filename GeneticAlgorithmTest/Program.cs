using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genetics.Generic;
using Genetics.Specialized;
using Genetics;

namespace GeneticAlgorithmTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BinaryChromosome.CrossoverOperator = new OnePointCrossover();
            BinaryChromosome.MutationOperator = new ClassicMutation();
            BinaryChromosome.Evaluator = new MyEvaluator();
            GeneticAlgorithm a = new GeneticAlgorithm(new BinaryChromosomeFactory(10), 50);
            a.Selector = new TournamentSelector();
            a.MaxIterations = 1000;

            a.Start();
            Console.WriteLine(a.BestChromosome.Value);
            a.Start();
            Console.WriteLine(a.BestChromosome.Value);
            a.Start();
            Console.WriteLine(a.BestChromosome.Value);
            a.Start();
            Console.WriteLine(a.BestChromosome.Value);
            Console.ReadKey();
        }
    }

    public class MyEvaluator : IEvaluator<List<bool>>
    {
        public double Eval(List<bool> genotype)
        {
            double result = 0;

            for (int i = 0; i < genotype.Count; i++)
            {
                if (genotype[i])
                    result += Math.Pow(i, 2);
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genetics;
using Genetics.Mutations;
using Genetics.Crossovers;
using Genetics.Evaluators;
using Genetics.Selectors;

namespace AHMED
{
    /// <summary>
    /// Główna aplikacja, korzystająca z osobnych bibliotek
    /// zespalając program w jedną całość.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to AHMED.");

            for (int i = 0; i < 10; i++)
            {
                LetsGeneticShiftPlusOne();
            }

            Console.WriteLine();


            Console.ReadKey();
        }

        static void LetsGeneticShiftPlusOne() {
            Chromosome.MutationOperator = new ClassicMutation();
            Chromosome.CrossoverOperator = new OnePointCrossover();
            Chromosome.Evaluator = new TestEvaluator();
            //Generation.Selector = new TournamentSelector();
            Generation.Selector = new RouletteSelector();

            Generation g = new Generation(100);
            g.MaxNumber = 500;
            g.MutationProbability = 0.001;
            g.CrossoverProbability = 0.75;

            g.Initiate(100);
            while (!g.Next()) { }
            Console.WriteLine("Best chromosome: {0} ({1})", g.BestChromosome.Value, g.BestChromosome);
        }
    }
}

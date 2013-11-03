using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics
{
    public class GeneticAlgorithm
    {
        private const double DEFAULT_CROSSOVER_PROBABILITY = 0.75;
        private const int DEFAULT_MAX_ITERATIONS = 100;
        private const int DEFAULT_POP_SIZE = 50;

        private int _currentIteration;
        
        private IChromosomeFactory _factory;
        
        private Population _currentPopulation;
        private Population _parentPopulation;
        private int _initialPopulationSize;

        private Random _randomizer;

        public GeneticAlgorithm(IChromosomeFactory factory, int initialPopulationSize = DEFAULT_POP_SIZE)
        {
            MaxIterations = DEFAULT_MAX_ITERATIONS;
            CrossoverProbability = DEFAULT_CROSSOVER_PROBABILITY;

            _initialPopulationSize = initialPopulationSize;
            _factory = factory;
            _randomizer = new Random();

            if (CheckStopCondition == null)
                CheckStopCondition = (p1, p2) => false;
        }

        public IChromosome BestChromosome { get { return _currentPopulation.BestChromosome; } }

        public Func<Population, Population, bool> CheckStopCondition { get; set; }

        public double CrossoverProbability { get; set; }

        public int CurrentIteration { get { return _currentIteration; } }

        /// <summary>
        /// Max iterations for genetic algorithm. Default 1000.
        /// </summary>
        public int MaxIterations { get; set; }

        /// <summary>
        /// Selector which creates new parent population.
        /// </summary>
        public ISelector Selector { get; set; }

        public void InitPopulation()
        {
            _currentPopulation = new Population();
            for (int i = 0; i < _initialPopulationSize; i++)
                _currentPopulation.Chromosomes.Add(_factory.Create());

            _currentPopulation.Repair();
            _currentPopulation.Eval();
        }

        public void NextGeneration()
        {
            _currentIteration++;

            _parentPopulation = _currentPopulation;

            // Selection.
            _currentPopulation = Selector.Select(_parentPopulation);

            // Crossover.
            List<IChromosome> toCrossover = new List<IChromosome>();

            for (int i = 0; i < _currentPopulation.Count; i++)
            {
                if (_randomizer.NextDouble() < CrossoverProbability)
                    // Insert at random position.
                    toCrossover.Insert(_randomizer.Next(0, toCrossover.Count), _currentPopulation[i]);
            }

            // Check if we can pair chromosomes
            // and optionally remove one.
            if (toCrossover.Count % 2 != 0)
                toCrossover.RemoveAt(_randomizer.Next(0, toCrossover.Count));

            // Chromosomes are in random order, so we can
            // pair them sequentially.
            for (int i = 0; i < toCrossover.Count; i += 2)
                toCrossover[i].Crossover(toCrossover[i + 1]);

            // Mutation.
            _currentPopulation.Chromosomes.ForEach(x => x.Mutate());

            // Reparation and evaluation.
            _currentPopulation.Repair();
            _currentPopulation.Eval();
        }

        public void Start()
        {
            InitPopulation();

            do 
            {
                NextGeneration();
            } while (!CheckStopCondition(_currentPopulation, _parentPopulation) && _currentIteration < MaxIterations);
        }
    }
}

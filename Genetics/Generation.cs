#define DIAGNOSTICS

using Genetics.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genetics.Repairers;
using System.Diagnostics;



namespace Genetics
{
    /// <summary>
    /// The most general genetic alghoritm class.
    /// Contains population, selector, evaluator...
    /// </summary>
    public class Generation
    {
        private const double DEFAULT_MUTATION_PROBABILITY = 0.01;
        private const double DEFAULT_CROSSOVER_PROBABILITY = 0.75;
        private const int DEFAULT_MAX_NUMBER = 1000;

        /// <summary>
        /// Avarage population fitness (population fitness
        /// divided by population size).
        /// </summary>
        private double _avgPopulationFitness;

        /// <summary>
        /// The best chromosome from all generations.
        /// </summary>
        private Chromosome _bestChromosome;

        /// <summary>
        /// Chromosome length.
        /// </summary>
        private int _chromosomeLength;

        /// <summary>
        /// Probability of performing a chromosome crossover.
        /// </summary>
        private double _crossoverProbability;

        /// <summary>
        /// Current population.
        /// </summary>
        private List<Chromosome> _currentPopulation;

        /// <summary>
        /// For diagnostics purposes - true if already found first chromosome
        /// where all people escaped.
        /// </summary>
        private bool _foundFirstCorrect = false;

        /// <summary>
        /// Upper limit of generations (stop condition).
        /// </summary>
        private int _maxNumber;

        /// <summary>
        /// Probability of single bit mutation in chromosome.
        /// </summary>
        private double _mutationProbability;

        /// <summary>
        /// Number of generation.
        /// </summary>
        private int _number;

        /// <summary>
        /// Parent population (chosen by selector).
        /// </summary>
        private List<Chromosome> _parentPopulation;

        /// <summary>
        /// Population fitness (sum of chromosomes' values).
        /// </summary>
        private double _populationFitness;

        private Random _randomizer;

        /// <summary>
        /// For diagnostics purposes.
        /// </summary>
        private Stopwatch[] _stopwatches;

        public Generation(int chromosomeLength)
        {
            _crossoverProbability = DEFAULT_CROSSOVER_PROBABILITY;
            _mutationProbability = DEFAULT_MUTATION_PROBABILITY;
            _chromosomeLength = chromosomeLength;
            _number = 1;
            _maxNumber = DEFAULT_MAX_NUMBER;
            _randomizer = new Random();

            // Empty chromosome (value = 0)
            _bestChromosome = new Chromosome();

            _stopwatches = new Stopwatch[3];
            for (int i = 0; i < _stopwatches.Length; i++)
                _stopwatches[i] = new Stopwatch();
        }

        /// <summary>
        /// Repairer used to fix chromosomes. Optional - can be null.
        /// </summary>
        public static IRepairer Repairer { get; set; }

        /// <summary>
        /// Selector used to perform selection on current population.
        /// </summary>
        public static ISelector Selector { get; set; }

        /// <summary>
        /// Avarage population fitness.
        /// </summary>
        public double AvrPopulationFitness
        {
            get { return _avgPopulationFitness; }
        }

        /// <summary>
        /// The best chromosome form all generations.
        /// </summary>
        public Chromosome BestChromosome
        {
            get { return _bestChromosome; }
        }

        /// <summary>
        /// Probability of performing a chromosome crossover.
        /// </summary>
        public double CrossoverProbability
        {
            get { return _crossoverProbability; }
            set { _crossoverProbability = value; }
        }

        /// <summary>
        /// Current population.
        /// </summary>
        public List<Chromosome> CurrentPopulation
        {
            get { return _currentPopulation; }
        }

        /// <summary>
        /// Max number of generations (stop condition).
        /// </summary>
        public int MaxNumber
        {
            get { return _maxNumber; }
            set { _maxNumber = value; }
        }

        /// <summary>
        /// Probability of single bit mutation in chromosome.
        /// </summary>
        public double MutationProbability
        {
            get { return _mutationProbability; }
            set { _mutationProbability = value; }
        }

        /// <summary>
        /// Number of generation increased by Next() method.
        /// </summary>
        public int Number
        {
            get { return _number; }
        }

        /// <summary>
        /// Parent population after selection.
        /// </summary>
        public List<Chromosome> ParentPopulation
        {
            get { return _parentPopulation; }
        }

        /// <summary>
        /// Population fitness.
        /// </summary>
        public double PopulationFitness
        {
            get { return _populationFitness; }
        }

        public void ApplyCrossover()
        {
            List<Chromosome> toCrossover = new List<Chromosome>();

            for (int i = 0; i < _parentPopulation.Count; i++)
            {
                if (_randomizer.NextDouble() < _crossoverProbability)
                    // Insert at random position.
                    toCrossover.Insert(_randomizer.Next(0, toCrossover.Count),_parentPopulation[i]);
            }

            // Check if we can pair chromosomes
            // and optionally remove one.
            if (toCrossover.Count % 2 != 0)
                toCrossover.RemoveAt(_randomizer.Next(0, toCrossover.Count));

            // Chromosomes are in random order, so we can
            // pair them sequentially.
            for (int i = 0; i < toCrossover.Count; i += 2)
                toCrossover[i].CrossAndReplace(toCrossover[i + 1]);
        }

        public void ApplyMutation()
        {
            for (int i = 0; i < _parentPopulation.Count; i++)
            {
                _parentPopulation[i].Mutate(_mutationProbability);
            }
        }

        /// <summary>
        /// Checks for stop condition.
        /// </summary>
        /// <returns>True if stop condition is fulfilled.</returns>
        public bool CheckStopCondition()
        {
            if (_number < _maxNumber)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Parent population becomes current population.
        /// </summary>
        public void CreateNewCurrentPopulation()
        {
            _currentPopulation = _parentPopulation;
            _parentPopulation = null;
        }

        /// <summary>
        /// Evaluates each chromosome value from current population.
        /// Calculates overall and avarage population fitness.
        /// </summary>
        public void Evaluate()
        {
            // Evaluate each chromosome and calculate overall population fitness.
            _populationFitness = 0;
            for (int i = 0; i < _currentPopulation.Count; i++)
            {
                _currentPopulation[i].Evaluate();
                _populationFitness += _currentPopulation[i].Value;

                if (_currentPopulation[i].CompareTo(_bestChromosome) == 1)
                {
                    _bestChromosome = new Chromosome(_currentPopulation[i]);
                    if (_bestChromosome.Value >= 16 && _foundFirstCorrect == false)
                    {
                        Console.WriteLine("Found first correct chromosome at generation {0}", _number);
                        _foundFirstCorrect = true;
                    }
                }
            }

            // Calculate avarage population fitness.
            _avgPopulationFitness = _populationFitness / _currentPopulation.Count;
        }

        /// <summary>
        /// Initiates first generation with specified population size.
        /// </summary>
        /// <param name="popsize">Initial population size.</param>
        public void Initiate(int popsize)
        {
            _currentPopulation = new List<Chromosome>(popsize);

            for (int i = 0; i < popsize; i++)
                _currentPopulation.Add(Chromosome.CreateRandom(_chromosomeLength));
            Repair();
            Evaluate();
        }

        /// <summary>
        /// Performs whole genetic algorithm cycle:
        /// evaluation, check for stop condition,
        /// selection, applying genetic operators,
        /// and creating new population.
        /// Increases generation number.
        /// 
        /// It won't create new population if stop condition
        /// is fulfilled.
        /// </summary>
        /// <returns>True if algorithm is finished (stop condition has been achieved).</returns>
        public bool Next()
        {
            if (CheckStopCondition())
                return true;

#if (DIAGNOSTICS)
            _stopwatches[0].Start();
            Select();
            ApplyCrossover();
            ApplyMutation();
            CreateNewCurrentPopulation();
            _stopwatches[0].Stop();
            _stopwatches[1].Start();
            Repair();
            _stopwatches[1].Stop();
            _stopwatches[2].Start();
            Evaluate();
            _stopwatches[2].Stop();
#else
            Select();
            ApplyCrossover();
            ApplyMutation();
            CreateNewCurrentPopulation();
            Repair();
            Evaluate();
#endif

            _number++;

            if (_number % 10 == 0)
            {
                int progress = _number * 100 / _maxNumber;

                long total = 0;
                for(int i = 0; i < _stopwatches.Length; i++)
                    total += _stopwatches[i].ElapsedMilliseconds;

                // Progress
                int top = Console.CursorTop;

                Console.CursorLeft = 0;
                Console.WriteLine("Progress: {0}%", progress);
#if (DIAGNOSTICS)
                Console.WriteLine("Power balance:");
                Console.WriteLine("Selection and operators: {0}%", _stopwatches[0].ElapsedMilliseconds * 100 / total);
                Console.WriteLine("Repair: {0}%", _stopwatches[1].ElapsedMilliseconds * 100 / total);
                Console.WriteLine("Evaluation (simulation): {0}%", _stopwatches[2].ElapsedMilliseconds * 100 / total);
#endif

                Console.CursorTop = top;
            }


            return false;
        }

        /// <summary>
        /// Prints genotypes all currentPopulation chromosomes.
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < _currentPopulation.Count; i++)
                Console.WriteLine(_currentPopulation[i]);
        }

        /// <summary>
        /// Repairs current population - if used should be called before evaluation.
        /// </summary>
        public void Repair()
        {
            if (Repairer == null)
                return;

            for (int i = 0; i < _currentPopulation.Count; i++)
                Repairer.RepairAndReplace(_currentPopulation[i]);
        }

        /// <summary>
        /// Performs selection from current population into parent population.
        /// </summary>
        /// <exception cref="SelectorException">May occur during selections.</exception>
        public void Select()
        {
            if (Selector == null)
                throw new SelectorException("No selector defined!");

            _parentPopulation = Selector.Select(_currentPopulation);
        }
    }
}

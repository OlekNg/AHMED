using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics
{
    public class Population
    {
        private const int DEFAUTL_POPULATION_SIZE = 50;

        private double _avgFitness;
        private double _fitness;
        private IChromosome _bestChromosome;

        public Population()
        {
            Chromosomes = new List<IChromosome>();
        }

        public Population(List<IChromosome> chromosomes)
        {
            Chromosomes = chromosomes;
        }

        public Population(int size)
        {
            Chromosomes = new List<IChromosome>(size);
        }

        public double AvgFitness
        {
            get
            {
                if (_avgFitness == default(double))
                    _avgFitness = CalculateAvgFitness();
                return _avgFitness;
            }
        }

        public IChromosome BestChromosome { get { return _bestChromosome; } }

        public double Fitness
        {
            get
            {
                if (_fitness == default(double))
                    _fitness = CalculateFitness();
                return _fitness;
            }
        }

        public int Count { get { return Chromosomes.Count; } }

        public List<IChromosome> Chromosomes { get; set; }

        public IChromosome this[int i]
        {
            get { return Chromosomes[i]; }
            set { Chromosomes[i] = value; }
        }

        private double CalculateAvgFitness()
        {
            return Fitness / Count;
        }

        private double CalculateFitness()
        {
            return Chromosomes.Sum(x => x.Value);
        }

        public void Add(IChromosome c)
        {
            Chromosomes.Add(c);
        }

        public void Eval()
        {
            Chromosomes.ForEach(x => x.Eval());
            IChromosome candidate = Chromosomes.Where(x => x.Value == Chromosomes.Max(y => y.Value)).First();

            if (_bestChromosome == null)
            {
                _bestChromosome = candidate;
                return;
            }

            if (candidate.Value > _bestChromosome.Value)
                _bestChromosome = candidate;
        }

        public void Repair()
        {
            Chromosomes.ForEach(x => x.Repair());
        }
    }
}

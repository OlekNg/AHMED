using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Specialized
{
    public class BinaryChromosomeFactory : IChromosomeFactory
    {
        private int _chromosomeLength;

        private Random _randomizer;

        public BinaryChromosomeFactory(int chromosomeLength)
        {
            _randomizer = new Random();
            _chromosomeLength = chromosomeLength;
        }

        public IChromosome Create()
        {
            List<bool> genotype = new List<bool>(_chromosomeLength);

            for (int i = 0; i < _chromosomeLength; i++)
                genotype.Add(_randomizer.Next(2) == 0 ? true : false);

            return new BinaryChromosome(genotype);
        }
    }
}

using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics.Specialized
{
    public class BinaryChromosome : Chromosome<List<bool>>
    {
        static BinaryChromosome()
        {
            CreateFunc = x => new BinaryChromosome(x);
        }

        public BinaryChromosome()
        {
            Genotype = new List<bool>();
        }

        public BinaryChromosome(List<bool> genotype)
        {
            Genotype = genotype;
        }
    }
}
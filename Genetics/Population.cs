using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics
{
    public class Population
    {
        private List<Chromosome> _population;

        private Chromosome _best;

        public Population()
        {

        }

        public Chromosome BestChromosome
        {
            get { return _best; }
        }

        public List<Chromosome> Chromosomes
        {
            get { return _population; }
        }

        public void Initiate()
        {

        }

        public void Eval()
        {

        }

        public void Select()
        {

        }

        public void Cross()
        {

        }

        public void Mutate()
        {

        }

        public void CreateNewPopulation()
        {

        }

        public bool CheckStopCondition()
        {
            return false;
        }
    }
}

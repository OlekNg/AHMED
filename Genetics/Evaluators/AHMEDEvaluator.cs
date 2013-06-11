using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulation;

namespace Genetics.Evaluators
{
    public class AHMEDEvaluator : IEvaluator
    {
        private Simulator _simulator;

        public AHMEDEvaluator(Simulator simulator)
        {
            _simulator = simulator;
        }

        public double Evaluate(Chromosome c)
        {
            List<EscapedGroup> result = _simulator.Simulate(c.Fenotype);

            int peopleCount = 16;

            // Calculate avarage.
            double sum = 0;
            int sumTicks = 0;
            int peopleEscaped = 0;

            foreach (EscapedGroup g in result)
            {
                sum += g.Quantity * g.Ticks;
                sumTicks += (int)g.Ticks;
                peopleEscaped += (int)g.Quantity;
            }

//             if (peopleEscaped > 0)
//             {
//                 Console.WriteLine("Escaped: {0}", peopleEscaped);
//             }

            double avg;
            if (peopleEscaped != 16)
                avg = 0;
            else
            {
                //Console.WriteLine("Everyone escaped!");
                avg = sum / (double)sumTicks;
            }
            return (double)peopleEscaped + avg;
            //return avg;
        }
    }
}

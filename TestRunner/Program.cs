using Genetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("First parameter is xml configuration file.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Loading xml file");
            var simulation = new EvacuationSimulation(args[0]);
            Console.WriteLine("Starting simulation");
            simulation.Start();
            Console.WriteLine("Simulation ended");
            Console.ReadKey();
        }
    }
}

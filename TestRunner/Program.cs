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

            var session = new SimulationSession(args[0]);
            
            Console.WriteLine("All simulations ended");
            Console.ReadKey();
        }
    }
}

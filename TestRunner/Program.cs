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
            var today = DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss");
            for (int i = 0; i < 5; i++)
            {
                var reader = new EvacuationSimulation.XmlConfigurationReader(args[0]);
                var simulation = new EvacuationSimulation(reader.GetBuilding(), reader.GetGeneticsConfiguration());
                simulation.StatisticsOutputPath = String.Format("session_{0}/pass_{1}", today, i + 1);
                Console.WriteLine("Simulation pass {0}", i + 1);
                simulation.Start();
            }
            
            Console.WriteLine("All simulations ended");
            Console.ReadKey();
        }
    }
}

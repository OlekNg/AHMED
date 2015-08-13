using Genetics;
using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestRunner
{
    class SimulationSession
    {
        private const int SimulationsInSession = 10;
        private double bestChromosomeValue = Double.MinValue;
        private int bestSessionChromosome = -1;
        private string xmlConfigurationPath;
        private string basePath;
        private string sessionFolder;
        private XmlDocument xmlDocument;
        private EvacuationSimulation.XmlConfigurationReader xmlConfiguration;

        public SimulationSession(string xmlConfigurationPath)
        {
            this.xmlConfigurationPath = xmlConfigurationPath;
            this.basePath = Path.GetDirectoryName(xmlConfigurationPath);
            this.xmlDocument = new XmlDocument();
            this.xmlDocument.Load(xmlConfigurationPath);
        }

        public SimulationSession(XmlDocument xmlDocument, string basePath)
        {
            this.xmlDocument = xmlDocument;
            this.basePath = basePath;
        }

        public void Run()
        {
            ReadConfiguration();
            CreateSessionFolder();
            CopyBuildingAndConfigurationToSessionFolder();
            PerformSession();
        }

        private void ReadConfiguration()
        {
            xmlConfiguration = new EvacuationSimulation.XmlConfigurationReader(xmlDocument, basePath);
        }

        private void CreateSessionFolder()
        {
            var today = DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss");
            sessionFolder = xmlConfiguration.SimulationName ?? String.Format("session_{0}", today);
            Directory.CreateDirectory(sessionFolder);
        }

        private void CopyBuildingAndConfigurationToSessionFolder()
        {
            var buildingPath = xmlConfiguration.GetBuildingPath();
            File.Copy(buildingPath, Path.Combine(sessionFolder, "Building.xml"));
            xmlDocument.Save(Path.Combine(sessionFolder, "Scenario.xml"));
        }

        private void PerformSession()
        {
            Console.WriteLine("Running session on {0}", xmlConfigurationPath);
            for (int i = 0; i < SimulationsInSession ;i++)
            {
                var simulation = new EvacuationSimulation(xmlConfiguration.GetBuilding(), xmlConfiguration.GetGeneticsConfiguration());
                simulation.StatisticsOutputPath = String.Format("{0}/pass_{1}", sessionFolder, i + 1);
                Console.WriteLine("Simulation pass {0}", i + 1);
                simulation.Start();

                var statistics = simulation.GetStatistics();
                if (statistics.BestChromosomeValue > bestChromosomeValue)
                {
                    bestChromosomeValue = statistics.BestChromosomeValue;
                    bestSessionChromosome = i;
                }
            }

            File.Copy(Path.Combine(sessionFolder, String.Format("pass_{0}", bestSessionChromosome + 1), "best_chromosome.txt"), Path.Combine(sessionFolder, "best_chromosome.txt"));
        }
    }
}

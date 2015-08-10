using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestRunner
{
    public class SimulationCollection
    {
        private string xmlPath;
        private string basePath;

        public SimulationCollection(string xmlPath)
        {
            this.xmlPath = xmlPath;
            this.basePath = Path.GetDirectoryName(xmlPath);
        }

        public void Run()
        {
            Console.WriteLine("Running simulation collection from file {0}", xmlPath);
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlPath);

            var nodes = xmlDocument.GetElementsByTagName("Simulation");
            Console.WriteLine("Found {0} simulations", nodes.Count);
            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var simulationDocument = new XmlDocument();
                simulationDocument.LoadXml(node.OuterXml);

                var simulation = new SimulationSession(simulationDocument, basePath);
                simulation.Run();
            }
        }
    }
}

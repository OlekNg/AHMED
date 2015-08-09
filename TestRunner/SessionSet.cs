using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestRunner
{
    public class SessionSet
    {
        private string xmlPath;
        private List<string> configPaths;

        public SessionSet(string path)
        {
            this.xmlPath = path;
        }

        public void Run()
        {
            LoadConfigPaths();
            ValidateIfEachConfigExists();
            RunSimulations();
        }

        private void LoadConfigPaths()
        {
            configPaths = new List<string>();
            var xml = new XmlDocument();
            xml.Load(xmlPath);

            var nodes = xml.GetElementsByTagName("Session");
            for (var i = 0; i < nodes.Count; i++)
            {
                configPaths.Add(nodes[i].Attributes.GetNamedItem("Config").InnerText);
            }
            Console.WriteLine("Loaded session set - {0} configs", configPaths.Count);
        }

        private void ValidateIfEachConfigExists()
        {
            foreach (var path in configPaths)
            {
                if (!File.Exists(path))
                    throw new ArgumentNullException(String.Format("Config file {0} does not exists!", path));
            }
        }

        private void RunSimulations()
        {
            foreach (var path in configPaths)
            {
                var session = new SimulationSession(path);
                session.Run();
            }
        }
    }
}

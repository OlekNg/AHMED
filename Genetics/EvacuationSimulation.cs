using BuildingEditor.ViewModel;
using Genetics.Evaluators;
using Genetics.Repairers;
using Genetics.Specialized;
using Genetics.Statistics;
using Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Genetics
{
    /// <summary>
    /// Represents single simulation of building evacuation.
    /// </summary>
    public class EvacuationSimulation
    {
        private GeneticsConfiguration<List<bool>> _geneticsConfiguration;
        private Building _building;
        private AlgorithmStatistics _statistics;

        public GeneticAlgorithm GeneticAlgorithm { get; protected set; }

        public EvacuationSimulation(string xmlConfigurationFile)
        {
            var xmlReader = new XmlConfigurationReader(xmlConfigurationFile);
            _building = xmlReader.GetBuilding();
            _geneticsConfiguration = xmlReader.GetGeneticsConfiguration();

            Setup();
        }

        public EvacuationSimulation(Building building, GeneticsConfiguration<List<bool>> geneticsConfiguration)
        {
            _building = building;
            _geneticsConfiguration = geneticsConfiguration;

            Setup();
        }

        public void Start()
        {
            _statistics = new AlgorithmStatistics(DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss"));
            GeneticAlgorithm.Start();
            _statistics.Dump();
        }

        public void Stop()
        {
            GeneticAlgorithm.Stop();
        }

        private void Setup()
        {
            _building.ShortGenotype = _geneticsConfiguration.ShortGenotype;
            if (_building.ShortGenotype)
            {
                _building.Rooms
                    .Where(x => x.NumberOfDoors == 1)
                    .ToList()
                    .ForEach(x => x.ApplySimpleEvacuation());
            }

            MapBuilder mapBuilder = new MapBuilder(_building.ToDataModel());
            Simulator sim = new Simulator();

            sim.MaximumTicks = _building.GetFloorCount() * 2;
            sim.SetupSimulator(mapBuilder.BuildBuildingMap(), mapBuilder.BuildPeopleMap());
            EvaCalcEvaluator evaluator = new EvaCalcEvaluator(sim, new Building(_building));

            BinaryChromosome.CrossoverOperator = _geneticsConfiguration.CrossoverOperator;
            BinaryChromosome.MutationOperator = _geneticsConfiguration.MutationOperator;
            BinaryChromosome.Transformer = _geneticsConfiguration.Transformer;
            BinaryChromosome.Repairer = new AdvancedRepairer(new Building(_building));
            BinaryChromosome.Evaluator = evaluator;

            GeneticAlgorithm = new GeneticAlgorithm(new BinaryChromosomeFactory(_building.GetFloorCount() * 2), _geneticsConfiguration.InitialPopulationSize);
            GeneticAlgorithm.Selector = _geneticsConfiguration.Selector;
            GeneticAlgorithm.MaxIterations = _geneticsConfiguration.MaxIterations;
            GeneticAlgorithm.ReportStatus += CollectAlgorithmStatus;
        }

        private void CollectAlgorithmStatus(GeneticAlgorithmStatus status)
        {
            _statistics.Collect(status);
        }

        public class XmlConfigurationReader
        {
            // Result members
            private Building building;
            private GeneticsConfiguration<List<bool>> geneticsConfiguration;

            // Local members
            private string path;
            private XmlDocument xmlDocument;

            public XmlConfigurationReader(string path)
            {
                this.path = path;
                LoadXmlDocument();
                LoadBuilding();
                CreateGeneticsConfiguration();
            }

            private void LoadXmlDocument()
            {
                xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
            }

            private void LoadBuilding()
            {
                var buildingNode = xmlDocument.GetElementsByTagName("Building").Item(0);
                var buildingPath = buildingNode.Attributes.GetNamedItem("Path").InnerText;
                var buildingCommonModel = new Common.DataModel.Building();
                buildingCommonModel.Load(buildingPath);
                building = new Building(buildingCommonModel);
            }

            private void CreateGeneticsConfiguration()
            {
                SetGeneralSettings();
                CreateSelector();
                CreateCrossoverOperator();
                CreateMutationOperator();
                CreateLocalOptimizationOperator();

            }

            private void SetGeneralSettings()
            {
                var generalNode = xmlDocument.GetElementsByTagName("General").Item(0);
                geneticsConfiguration.InitialPopulationSize = Int32.Parse(generalNode.Attributes.GetNamedItem("PopulationSize").InnerText);
                geneticsConfiguration.MaxIterations = Int32.Parse(generalNode.Attributes.GetNamedItem("MaxIterations").InnerText);
                geneticsConfiguration.ShortGenotype = Boolean.Parse(generalNode.Attributes.GetNamedItem("ShortGenotype").InnerText);
            }

            private void CreateSelector()
            {
                switch (GetAttributeValueOfElement("Type", "Selector"))
                {
                    case "Tournament":
                        geneticsConfiguration.Selector = new TournamentSelector();
                        break;
                    default:
                        throw new ArgumentException("Invalid selector name");
                }
            }

            private void CreateCrossoverOperator()
            {
                switch (GetAttributeValueOfElement("Type", "Crossover"))
                {
                    case "OnePoint":
                        geneticsConfiguration.CrossoverOperator = new OnePointCrossover();
                        break;
                    default:
                        throw new ArgumentException("Invalid crossover name");
                }
            }

            private void CreateMutationOperator()
            {
                switch (GetAttributeValueOfElement("Type", "Mutation"))
                {
                    case "Classic":
                        geneticsConfiguration.MutationOperator = new ClassicMutation();
                        break;
                    default:
                        throw new ArgumentException("Invalid mutation name");
                }
            }

            private void CreateLocalOptimizationOperator()
            {
                switch (GetAttributeValueOfElement("Type", "Transformer"))
                {
                    case "None":
                        geneticsConfiguration.Transformer = null;
                        break;
                    default:
                        throw new ArgumentException("Invalid transformer name");
                }
            }

            private string GetAttributeValueOfElement(string attributeName, string elementName)
            {
                return xmlDocument.GetElementsByTagName(elementName).Item(0).Attributes.GetNamedItem(attributeName).InnerText;
            }

            public Building GetBuilding()
            {
                return building;
            }

            public GeneticsConfiguration<List<bool>> GetGeneticsConfiguration()
            {
                return geneticsConfiguration;
            }
        }
    }
}

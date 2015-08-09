using BuildingEditor.ViewModel;
using Genetics.Evaluators;
using Genetics.Operators;
using Genetics.Repairers;
using Genetics.Specialized;
using Genetics.Statistics;
using Simulation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

        public string StatisticsOutputPath { get; set; }
        public GeneticAlgorithm GeneticAlgorithm { get; protected set; }

        public EvacuationSimulation(Building building, GeneticsConfiguration<List<bool>> geneticsConfiguration)
        {
            _building = building;
            _geneticsConfiguration = geneticsConfiguration;

            Setup();
        }

        public void Start()
        {
            _statistics = new AlgorithmStatistics(StatisticsOutputPath ?? DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss"));
            GeneticAlgorithm.Start();
            _statistics.Dump();
        }

        public AlgorithmStatistics GetStatistics()
        {
            return _statistics;
        }

        public void Stop()
        {
            GeneticAlgorithm.Stop();
        }

        private void Setup()
        {
            _building.ShortGenotype = _geneticsConfiguration.ShortGenotype;
            _building.ApplySimpleEvacuationIfShortGenotype();

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
            if (_geneticsConfiguration.MaxIterationsWithoutImprovement > 0)
                GeneticAlgorithm.EnableBestChromosomeBasedStopCondition(_geneticsConfiguration.MaxIterationsWithoutImprovement);
            GeneticAlgorithm.Selector = _geneticsConfiguration.Selector;
            GeneticAlgorithm.MaxIterations = _geneticsConfiguration.MaxIterations;
            GeneticAlgorithm.CrossoverProbability = _geneticsConfiguration.CrossoverProbability;
            GeneticAlgorithm.ReportStatus += CollectAlgorithmStatus;
        }

        private void CollectAlgorithmStatus(GeneticAlgorithmStatus status)
        {
            _statistics.Collect(status);
        }

        public class XmlConfigurationReader
        {
            // Result members
            private string buildingPath;
            private Building building;
            private GeneticsConfiguration<List<bool>> geneticsConfiguration;

            // Local members
            private string path;
            private string basePath;
            private XmlDocument xmlDocument;
            private string overrideBuildingPath;

            public XmlConfigurationReader(string path, string overrideBuildingPath = null)
            {
                this.path = path;
                basePath = Path.GetDirectoryName(path);
                this.overrideBuildingPath = overrideBuildingPath;
                LoadXmlDocument();
            }

            private void LoadXmlDocument()
            {
                xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
            }

            private void LoadBuilding()
            {
                LoadBuildingPath();
                var buildingCommonModel = new Common.DataModel.Building();
                buildingCommonModel.Load(buildingPath);
                building = new Building(buildingCommonModel);
            }

            private void LoadBuildingPath()
            {
                if (overrideBuildingPath != null)
                    buildingPath = overrideBuildingPath;
                else
                {
                    var buildingNode = xmlDocument.GetElementsByTagName("Building").Item(0);
                    buildingPath = Path.Combine(basePath, buildingNode.Attributes.GetNamedItem("Path").InnerText);
                }
            }

            private void CreateGeneticsConfiguration()
            {
                geneticsConfiguration = new GeneticsConfiguration<List<bool>>();
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

                var maxIterAttr = generalNode.Attributes.GetNamedItem("MaxIterationsWithoutImprovement");
                if (maxIterAttr != null)
                    geneticsConfiguration.MaxIterationsWithoutImprovement = Int32.Parse(maxIterAttr.InnerText);
            }

            private void CreateSelector()
            {
                switch (GetAttributeValueOfElement("Type", "Selector"))
                {
                    case "Tournament":
                        geneticsConfiguration.Selector = new TournamentSelector();
                        break;
                    case "Roulette":
                        geneticsConfiguration.Selector = new RouletteSelector();
                        break;
                    case "Rank":
                        geneticsConfiguration.Selector = new RankSelector();
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
                    case "MultiPoint":
                        geneticsConfiguration.CrossoverOperator = new MultiPointCrossover(Int32.Parse(GetAttributeValueOfElement("Points", "Crossover")));
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
                        geneticsConfiguration.MutationOperator = new ClassicMutation(Double.Parse(GetAttributeValueOfElement("Probability", "Mutation"), CultureInfo.InvariantCulture));
                        break;
                    case "PathDirection":
                        geneticsConfiguration.MutationOperator = new PathDirectionMutation(GetBuilding(), Double.Parse(GetAttributeValueOfElement("Probability", "Mutation"), CultureInfo.InvariantCulture));
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
                    case "ThreeSegmentLoopOptimizer":
                        geneticsConfiguration.Transformer = new ThreeSegmentLoopOptimizer(GetBuilding(), Double.Parse(GetAttributeValueOfElement("Probability", "Transformer"), CultureInfo.InvariantCulture));
                        break;
                    case "LocalOptimization":
                        geneticsConfiguration.Transformer = new LocalOptimization(GetBuilding(), GetEvaluator());
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
                if (building == null)
                    LoadBuilding();
                return building;
            }

            public GeneticsConfiguration<List<bool>> GetGeneticsConfiguration()
            {
                if (geneticsConfiguration == null)
                    CreateGeneticsConfiguration();
                return geneticsConfiguration;
            }

            public string GetBuildingPath()
            {
                if (building == null)
                    LoadBuilding();
                return buildingPath;
            }

            public EvaCalcEvaluator GetEvaluator()
            {
                MapBuilder mapBuilder = new MapBuilder(building.ToDataModel());
                Simulator sim = new Simulator();

                sim.MaximumTicks = building.GetFloorCount() * 2;
                sim.SetupSimulator(mapBuilder.BuildBuildingMap(), mapBuilder.BuildPeopleMap());
                return new EvaCalcEvaluator(sim, new Building(building));
            }
        }
    }
}

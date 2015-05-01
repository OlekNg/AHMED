using BuildingEditor.ViewModel;
using Genetics.Evaluators;
using Genetics.Repairers;
using Genetics.Specialized;
using Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetics
{
    /// <summary>
    /// Represents single simulation of building evacuation.
    /// </summary>
    public class EvacuationSimulation
    {
        private GeneticsConfiguration<List<bool>> _geneticsConfiguration;
        private Building _building;

        public GeneticAlgorithm GeneticAlgorithm { get; protected set; }

        public EvacuationSimulation(Building building, GeneticsConfiguration<List<bool>> geneticsConfiguration)
        {
            _building = building;
            _geneticsConfiguration = geneticsConfiguration;

            Setup();
        }

        public void Start()
        {
            GeneticAlgorithm.Start();
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
        }
    }
}

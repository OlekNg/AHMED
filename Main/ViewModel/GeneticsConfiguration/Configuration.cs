using BuildingEditor.ViewModel;
using Genetics;
using Genetics.Evaluators;
using Genetics.Repairers;
using Genetics.Specialized;
using PropertyChanged;
using Simulation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Main.ViewModel.GeneticsConfiguration
{
    [ImplementPropertyChanged]
    public class Configuration
    {
        public Configuration()
        {
            Selectors = new ObservableCollection<ISelectionConfiguration>();
            Selectors.Add(SelectedSelector = new TournamentSelectionConfiguration());
            Selectors.Add(new RankSelectionConfiguration());
            Selectors.Add(new RouletteSelectionConfiguration());

            Crossovers = new ObservableCollection<ICrossoverConfiguration>();
            Crossovers.Add(SelectedCrossover = new OnePointCrossoverConfiguration());
            Crossovers.Add(new MultiPointCrossoverConfiguration());

            Mutations = new ObservableCollection<IMutationConfiguration>();
            Mutations.Add(SelectedMutation = new ClassicMutationConfiguration());
            Mutations.Add(new PathDirectionMutationConfiguration());

            Transformers = new ObservableCollection<ITransformationConfiguration>();
            Transformers.Add(SelectedTransformer = new NoneTransformationConfiguration());
            Transformers.Add(new ThreeSegmentLoopOptimizerConfiguration());
            Transformers.Add(new LocalOptimizationConfiguration());

            // Default values
            MaxIterations = 500;
            InitPopSize = 50;
            CrossoverProbability = 0.75;
            ShortGenotype = false;
            MaxIterationsWithoutImprovement = 0;
        }

        // General
        public int MaxIterations { get; set; }
        public int InitPopSize { get; set; }
        public int MaxIterationsWithoutImprovement { get; set; }

        // Selection
        public ObservableCollection<ISelectionConfiguration> Selectors { get; set; }
        public ISelectionConfiguration SelectedSelector { get; set; }

        // Crossover
        public ObservableCollection<ICrossoverConfiguration> Crossovers { get; set; }
        public ICrossoverConfiguration SelectedCrossover { get; set; }
        public double CrossoverProbability { get; set; }

        // Mutation
        public ObservableCollection<IMutationConfiguration> Mutations { get; set; }
        public IMutationConfiguration SelectedMutation { get; set; }

        // Transformation
        public ObservableCollection<ITransformationConfiguration> Transformers { get; set; }
        public ITransformationConfiguration SelectedTransformer { get; set; }

        public bool ShortGenotype { get; set; }

        internal GeneticsConfiguration<List<bool>> BuildConfiguration(Building CurrentBuilding)
        {
            return new GeneticsConfiguration<List<bool>>()
            {
                CrossoverOperator = SelectedCrossover.BuildCrossoverOperator(CurrentBuilding),
                MutationOperator = SelectedMutation.BuildMutationOperator(CurrentBuilding),
                InitialPopulationSize = InitPopSize,
                MaxIterations = MaxIterations,
                Selector = SelectedSelector.BuildSelector(),
                Transformer = SelectedTransformer.BuildTransformer(CurrentBuilding),
                ShortGenotype = ShortGenotype,
                CrossoverProbability = CrossoverProbability,
                MaxIterationsWithoutImprovement = MaxIterationsWithoutImprovement
            };
        }
    }
}

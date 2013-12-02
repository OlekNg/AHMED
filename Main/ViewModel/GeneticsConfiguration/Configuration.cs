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

            // Default values
            MaxIterations = 500;
            InitPopSize = 50;
            CrossoverProbability = 0.75;
        }

        // General
        public int MaxIterations { get; set; }
        public int InitPopSize { get; set; }

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
    }
}

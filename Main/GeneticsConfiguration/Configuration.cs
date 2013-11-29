using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Main.GeneticsConfiguration
{
    [ImplementPropertyChanged]
    public class Configuration
    {
        public Configuration()
        {
            Crossovers = new ObservableCollection<ICrossoverConfiguration>();
            Crossovers.Add(SelectedCrossover = new OnePointCrossoverConfiguration());
            Crossovers.Add(new MultiPointCrossoverConfiguration());

            Selectors = new ObservableCollection<ISelectionConfiguration>();
            Selectors.Add(SelectedSelector = new TournamentSelectionConfiguration());
            Selectors.Add(new RankSelectionConfiguration());
            Selectors.Add(new RouletteSelectionConfiguration());

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
    }
}

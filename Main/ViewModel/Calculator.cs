using BuildingEditor.ViewModel;
using Genetics;
using Genetics.Evaluators;
using Genetics.Repairers;
using Genetics.Specialized;
using Main.View;
using Main.ViewModel.GeneticsConfiguration;
using PropertyChanged;
using Simulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Main.ViewModel
{
    /// <summary>
    /// View model for MainWindow.
    /// </summary>
    [ImplementPropertyChanged]
    public class Calculator
    {
        #region Fields
        /// <summary>
        /// Currently open file.
        /// </summary>
        private string _currentFile;

        /// <summary>
        /// Currently running genetic algorithm.
        /// </summary>
        private GeneticAlgorithm _currentGA;

        /// <summary>
        /// True - file with open building has been overwritten and should
        /// be reloaded.
        /// </summary>
        private bool _isFileDirty = false;

        /// <summary>
        /// Genetic algorithm view model configuration.
        /// </summary>
        private Configuration _geneticsConfiguration = new Configuration();

        /// <summary>
        /// Tracks open file for overwritting (for example by editor).
        /// </summary>
        private FileSystemWatcher _watcher;
        
        #endregion

        #region Calculator construction
        public Calculator()
        {
            InitFileWatcher();
        }

        private void InitFileWatcher()
        {
            _watcher = new FileSystemWatcher();
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.Path = "C:\\";
            _watcher.Filter = "*.xml";

            _watcher.Changed += new FileSystemEventHandler(OnFileChanged);

            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Checks if our currently open file has changed and marks it as dirty (if yes).
        /// </summary>
        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == CurrentFile)
                _isFileDirty = true;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Currently open building.
        /// </summary>
        public Building CurrentBuilding { get; set; }

        /// <summary>
        /// Path to currently open file.
        /// </summary>
        public string CurrentFile
        {
            get { return _currentFile; }
            set
            {
                _currentFile = value;
                _watcher.Path = System.IO.Path.GetDirectoryName(value);
            }
        }
        #endregion

        #region Actions
        public void CancelGA()
        {
            if (_currentGA != null)
                _currentGA.Stop();
        }

        /// <summary>
        /// Loads building definition from given xml file.
        /// </summary>
        /// <param name="file">Path to building definition file.</param>
        public void LoadBuilding(string file)
        {
            CurrentFile = file;
            Common.DataModel.Building building = new Common.DataModel.Building();
            building.Load(file);
            CurrentBuilding = new Building(building);
        }

        public void RunGA()
        {
            // If no building is currently loaded then do nothing.
            if (String.IsNullOrEmpty(CurrentFile))
                return;

            AdvancedRepairer r = new AdvancedRepairer(new Building(CurrentBuilding.ToDataModel()));

            MapBuilder mapBuilder = new MapBuilder(CurrentBuilding.ToDataModel());
            Simulator sim = new Simulator();

            sim.MaximumTicks = CurrentBuilding.GetFloorCount() * 2;
            sim.SetupSimulator(mapBuilder.BuildBuildingMap(), mapBuilder.BuildPeopleMap());
            AHMEDEvaluator evaluator = new AHMEDEvaluator(sim, new Building(CurrentBuilding.ToDataModel()));
            //AHMEDEvaluator evaluator = new AHMEDEvaluator(sim, _building); // works on actual building

            BinaryChromosome.CrossoverOperator = _geneticsConfiguration.SelectedCrossover.BuildCrossoverOperator();
            BinaryChromosome.MutationOperator = _geneticsConfiguration.SelectedMutation.BuildMutationOperator();
            BinaryChromosome.Evaluator = evaluator;
            BinaryChromosome.Repairer = r;
            GeneticAlgorithm ga = new GeneticAlgorithm(new BinaryChromosomeFactory(CurrentBuilding.GetFloorCount() * 2), _geneticsConfiguration.InitPopSize);
            ga.Selector = _geneticsConfiguration.SelectedSelector.BuildSelector();
            ga.MaxIterations = _geneticsConfiguration.MaxIterations;
            ga.Completed += OnGeneticCompleted;

            // Create view model for presenting progression of algorithm.
            Status statusModel = new Status(ga);
            StatusWindow statusWindow = new StatusWindow(ga.Stop);
            statusWindow.DataContext = statusModel;
            statusWindow.Show();

            // Run algorithm asynchronously.
            _currentGA = ga;
            new Task(ga.Start).Start();
        }

        public void SetViewMode(string viewMode)
        {
            CurrentBuilding.ViewMode = viewMode;
        }

        public void ShowGeneticsConfiguration()
        {
            GeneticsWindow window = new GeneticsWindow(_geneticsConfiguration);
            window.Show();
        }

        /// <summary>
        /// Reloads building from file if it was overwritten.
        /// </summary>
        internal void RefreshFileIfDirty()
        {
            // Refresh building if it has changed.
            if (_isFileDirty)
            {
                _isFileDirty = false;
                LoadBuilding(CurrentFile);
            }
        }
        #endregion

        /// <summary>
        /// Applies final solution to building view model.
        /// </summary>
        private void OnGeneticCompleted(GeneticAlgorithmStatus status)
        {
            CurrentBuilding.SetFenotype(((BinaryChromosome)status.BestChromosome).Genotype.ToFenotype());
            CurrentBuilding.DrawSolution();
        }
    }
}

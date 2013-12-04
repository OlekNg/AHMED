using BuildingEditor.ViewModel;
using BuildingEditor.ViewModel.Tools;
using Genetics;
using Genetics.Evaluators;
using Genetics.Repairers;
using Genetics.Specialized;
using Main.View;
using Main.ViewModel.GeneticsConfiguration;
using PropertyChanged;
using Simulation;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Main.ViewModel
{
    /// <summary>
    /// View model for MainWindow.
    /// </summary>
    [ImplementPropertyChanged]
    public class Calculator : ISegmentEventHandler
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
        /// Debug info for debug window.
        /// </summary>
        private DebugInfo _debugInfo;

        /// <summary>
        /// The only tool that can be used in main application.
        /// </summary>
        private Tool _dragTool;

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
        public Calculator(FrameworkElement workspace, FrameworkElement window)
        {
            _debugInfo = new DebugInfo();
            _dragTool = new DragTool(workspace, window);
            InitFileWatcher();
            CreateCommands();

            SegmentEventHandler.Register(this);
        }

        private void CreateCommands()
        {
            DebugInfoCommand = new SimpleCommand(x =>
            {
                DebugWindow wnd = new DebugWindow(_debugInfo);
                wnd.Show();
            });
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

        #region Commands
        public ICommand DebugInfoCommand { get; set; }
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
            _debugInfo.Building = CurrentBuilding;
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
            StatusWindow statusWindow = new StatusWindow();
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

        #region Segment events handlers.
        public void Segment_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _dragTool.MouseDown(sender, e);
        }

        public void Segment_MouseMove(object sender, MouseEventArgs e)
        {
            _dragTool.MouseMove(sender, e);
        }

        public void Segment_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _dragTool.MouseUp(sender, e);
        }

        public void Segment_MouseEnter(object sender, MouseEventArgs e)
        {
            _dragTool.MouseEnter(sender, e);
        }

        public void Segment_MouseLeave(object sender, MouseEventArgs e)
        {
            _dragTool.MouseLeave(sender, e);
        }
        #endregion

        public void Workspace_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _dragTool.MouseWheel(sender, e);
        }

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

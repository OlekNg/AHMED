using BuildingEditor.ViewModel;
using BuildingEditor.ViewModel.Tools;
using Common.DataModel.Enums;
using Genetics;
using Genetics.Evaluators;
using Genetics.Operators;
using Genetics.Repairers;
using Genetics.Specialized;
using Main.View;
using Main.ViewModel.GeneticsConfiguration;
using PropertyChanged;
using Simulation;
using System;
using System.Collections.Generic;
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
        private Building _currentBuilding;

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
        private Configuration _geneticsConfiguration;

        private Dictionary<Direction, Direction> _nextDirections;
        private Dictionary<Direction, Direction> _previousDirections;

        /// <summary>
        /// Tracks open file for overwritting (for example by editor).
        /// </summary>
        private FileSystemWatcher _watcher;
        
        #endregion

        #region Calculator construction
        public Calculator(FrameworkElement workspace, FrameworkElement window)
        {
            _geneticsConfiguration = new Configuration();
            _debugInfo = new DebugInfo();
            _dragTool = new DragTool(workspace, window);
            InitFileWatcher();
            CreateCommands();
            CreateDirections();

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

        private void CreateDirections()
        {
            _nextDirections = new Dictionary<Direction, Direction>();
            _nextDirections.Add(Direction.LEFT, Direction.UP);
            _nextDirections.Add(Direction.UP, Direction.RIGHT);
            _nextDirections.Add(Direction.RIGHT, Direction.DOWN);
            _nextDirections.Add(Direction.DOWN, Direction.LEFT);

            _previousDirections = new Dictionary<Direction, Direction>();
            _previousDirections.Add(Direction.LEFT, Direction.DOWN);
            _previousDirections.Add(Direction.UP, Direction.LEFT);
            _previousDirections.Add(Direction.RIGHT, Direction.UP);
            _previousDirections.Add(Direction.DOWN, Direction.RIGHT);
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
        public Building CurrentBuilding
        {
            get { return _currentBuilding; }
            set { _currentBuilding = value; }
        }

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

            var viewMode = CurrentBuilding != null ? CurrentBuilding.ViewMode : "";
            CurrentBuilding = new Building(building);
            CurrentBuilding.ViewMode = viewMode;

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
            EvaCalcEvaluator evaluator = new EvaCalcEvaluator(sim, new Building(CurrentBuilding.ToDataModel()));
            //AHMEDEvaluator evaluator = new AHMEDEvaluator(sim, _building); // works on actual building

            BinaryChromosome.CrossoverOperator = _geneticsConfiguration.SelectedCrossover.BuildCrossoverOperator(CurrentBuilding);
            BinaryChromosome.MutationOperator = _geneticsConfiguration.SelectedMutation.BuildMutationOperator(CurrentBuilding);
            BinaryChromosome.Transformer = _geneticsConfiguration.SelectedTransformer.BuildTransformer(CurrentBuilding);
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

            // Run algorithm asynchronously.
            _currentGA = ga;
            new Task(ga.Start).Start();

            statusWindow.ShowDialog();
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

        public void Segment_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Changing segment fenotype if solution mode
            if (CurrentBuilding.ViewMode == "Solution")
            {
                var element = sender as FrameworkElement;
                if (element == null) return;

                var segment = element.Tag as Segment;
                if (segment == null) return;

                if (e.Delta > 0)
                    segment.Fenotype = _nextDirections[segment.Fenotype];
                else
                    segment.Fenotype = _previousDirections[segment.Fenotype];

                CurrentBuilding.DrawSolution();
                _debugInfo.Update();
            }
            else
                _dragTool.MouseWheel(sender, e);
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

﻿using BuildingEditor.Logic;
using Common.DataModel.Enums;
using Genetics;
using Genetics.Evaluators;
using Genetics.Operators;
using Genetics.Repairers;
using Genetics.Specialized;
using Main.GeneticsConfiguration;
using Simulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Building _building;
        private string _currentFile;
        private bool _fileDirty;
        private Configuration _geneticsConfiguration = new Configuration();
        private FileSystemWatcher _watcher;

        public MainWindow()
        {
            InitializeComponent();
            InitFileWatcher();

            _building = new Building();
            uxWorkspaceViewbox.DataContext = _building;
            uxFloors.DataContext = _building;
        }

        

        public string CurrentFile
        {
            get { return _currentFile; }
            set {
                _currentFile = value;
                Title = String.Format("AHMED v2 - {0}", value);
                _watcher.Path = System.IO.Path.GetDirectoryName(value);
            }
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

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("{0} - {1}", e.FullPath, e.ChangeType);
            if(e.FullPath == _currentFile)
                _fileDirty = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadBuilding(string file)
        {
            Common.DataModel.Building building = new Common.DataModel.Building();
            building.Load(file);
            Building viewModel = new Building(building);

            _building.Floors = viewModel.Floors;
            _building.Stairs = viewModel.Stairs;
            _building.CurrentFloor = _building.Floors[0];
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".xml";
            dlg.Filter = "Building definition file|*.xml";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                CurrentFile = dlg.FileName;
                LoadBuilding(CurrentFile);
            }
        }

        private void ViewMode_Clicked(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            string mode = element.Tag as string;

            if (mode != null)
                _building.ViewMode = mode;
        }

        private void ProcessRun_Click(object sender, RoutedEventArgs e)
        {
            AdvancedRepairer r = new AdvancedRepairer(new Building(_building.ToDataModel()));

            MapBuilder mapBuilder = new MapBuilder(_building.ToDataModel());
            Simulator sim = new Simulator();

            sim.MaximumTicks = _building.GetFloorCount() * 2;
            sim.SetupSimulator(mapBuilder.BuildBuildingMap(), mapBuilder.BuildPeopleMap());
            AHMEDEvaluator evaluator = new AHMEDEvaluator(sim, new Building(_building.ToDataModel()));
            //AHMEDEvaluator evaluator = new AHMEDEvaluator(sim, _building); // works on actual building

            BinaryChromosome.CrossoverOperator = _geneticsConfiguration.SelectedCrossover.BuildCrossoverOperator();
            BinaryChromosome.MutationOperator = _geneticsConfiguration.SelectedMutation.BuildMutationOperator();
            BinaryChromosome.Evaluator = evaluator;
            BinaryChromosome.Repairer = r;
            GeneticAlgorithm ga = new GeneticAlgorithm(new BinaryChromosomeFactory(_building.GetFloorCount() * 2), _geneticsConfiguration.InitPopSize);
            ga.Selector = _geneticsConfiguration.SelectedSelector.BuildSelector();
            ga.MaxIterations = _geneticsConfiguration.MaxIterations;
            ga.ReportStatus += AlgorithmStatus;
            ga.Start();
            _building.SetFenotype(((BinaryChromosome)ga.BestChromosome).Genotype.ToFenotype());
            _building.DrawSolution();
        }

        private void AlgorithmStatus(GeneticAlgorithmStatus status)
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            Console.WriteLine("Iteration: {0}", status.IterationNumber);
            Console.WriteLine("Crossover overhead: {0:0.000}", status.CrossoverOverhead);
            Console.WriteLine("Evaluation overhead: {0:0.000}", status.EvaluationOverhead);
            Console.WriteLine("Mutation overhead: {0:0.000}", status.MutationOverhead);
            Console.WriteLine("Repair overhead: {0:0.000}", status.RepairOverhead);
            Console.WriteLine("Population fitness: {0:0.000}", status.CurrentPopulation.Fitness);
            Console.WriteLine("Population avg fitness: {0:0.000}", status.CurrentPopulation.AvgFitness);
            Console.WriteLine("Best population chromosome value: {0:0.000}", status.CurrentPopulation.BestChromosome.Value);
            Console.WriteLine("Best algorithm chromosome value: {0:0.000}", status.BestChromosome.Value);
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            List<Direction> fenotype = new List<Direction>() { Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.DOWN,
                                                               Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.UP };
            _building.SetFenotype(fenotype);
            _building.DrawSolution();

            MapBuilder mapBuilder = new MapBuilder(_building.ToDataModel());
            Simulator sim = new Simulator();

            sim.MaximumTicks = 50;
            sim.SetupSimulator(mapBuilder.BuildBuildingMap(), mapBuilder.BuildPeopleMap());
            var result = sim.Simulate(_building.GetSimulatorFenotype());

            foreach (var group in result)
            {
                Console.WriteLine("Escaped group: quantity {0}, ticks {1}", group.Quantity, group.Ticks);
            }
        }

        private void Test2_Click(object sender, RoutedEventArgs e)
        {
            List<Direction> fenotype = new List<Direction>() { Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.DOWN,
                                                               Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.DOWN,
                                                               Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.DOWN,
                                                               Direction.DOWN, Direction.LEFT, Direction.RIGHT, Direction.RIGHT, Direction.DOWN,
                                                               Direction.RIGHT, Direction.RIGHT, Direction.DOWN, Direction.LEFT, Direction.LEFT };
            _building.SetFenotype(fenotype);
            _building.DrawSolution();
        }

        private void ToolsEditor_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(typeof(BuildingEditor.App).Assembly.Location, CurrentFile);
        }

        private void ToolsGenetics_Click(object sender, RoutedEventArgs e)
        {
            GeneticsWindow window = new GeneticsWindow(_geneticsConfiguration);
            window.Show();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            // Refresh building if it has changed.
            if (_fileDirty)
            {
                _fileDirty = false;
                LoadBuilding(CurrentFile);
            }
        }

        
    }
}

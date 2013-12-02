using BuildingEditor.Logic;
using Common.DataModel.Enums;
using Genetics;
using Genetics.Evaluators;
using Genetics.Operators;
using Genetics.Repairers;
using Genetics.Specialized;
using Main.GeneticsConfiguration;
using Main.StatusControl;
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
        #region Fields
        /// <summary>
        /// Currently open building.
        /// </summary>
        private Building _building;

        /// <summary>
        /// Path to currently open file.
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
        private bool _isFileDirty;

        /// <summary>
        /// Genetic algorithm view model configuration.
        /// </summary>
        private Configuration _geneticsConfiguration = new Configuration();

        /// <summary>
        /// Tracks open file for overwritting (for example by editor).
        /// </summary>
        private FileSystemWatcher _watcher;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            InitFileWatcher();

            // Init empty building.
            _building = new Building();

            // Set data contexts.
            uxWorkspaceViewbox.DataContext = _building;
            uxFloors.DataContext = _building;
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
                Title = String.Format("AHMED v2 - {0}", value); // Update window title.
            }
        }

        /// <summary>
        /// Loads building from specified file and applies it to view model.
        /// </summary>
        /// <param name="file">Path to xml file with building definition.</param>
        private void LoadBuilding(string file)
        {
            CurrentFile = file;
            Common.DataModel.Building building = new Common.DataModel.Building();
            building.Load(file);
            Building viewModel = new Building(building);

            _building.Floors = viewModel.Floors;
            _building.Stairs = viewModel.Stairs;
            _building.CurrentFloor = _building.Floors[0];
        }

        /// <summary>
        /// Updates building view mode.
        /// </summary>
        private void ViewMode_Clicked(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            string mode = element.Tag as string;

            if (mode != null)
                _building.ViewMode = mode;
        }

        /// <summary>
        /// Applies final solution to building view model.
        /// </summary>
        private void OnGeneticCompleted(GeneticAlgorithmStatus status)
        {
            _building.SetFenotype(((BinaryChromosome)status.BestChromosome).Genotype.ToFenotype());
            _building.DrawSolution();
        }

        /// <summary>
        /// Reloads building from file if it was overwritten.
        /// </summary>
        private void Window_Activated(object sender, EventArgs e)
        {
            // Refresh building if it has changed.
            if (_isFileDirty)
            {
                _isFileDirty = false;
                LoadBuilding(CurrentFile);
            }
        }

        #region File watcher
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
            if (e.FullPath == _currentFile)
                _isFileDirty = true;
        }
        #endregion

        #region Menu actions
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                // Load building.
                LoadBuilding(dlg.FileName);
            }
        }

        private void ProcessCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_currentGA != null)
                _currentGA.Stop();
        }

        private void ProcessRun_Click(object sender, RoutedEventArgs e)
        {
            // If no building is currently loaded then do nothing.
            if (String.IsNullOrEmpty(CurrentFile))
                return;

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
            ga.Completed += OnGeneticCompleted;

            // Create view model for presenting progression of algorithm.
            StatusViewModel statusModel = new StatusViewModel(ga);
            StatusWindow statusWindow = new StatusWindow(ga.Stop);
            statusWindow.DataContext = statusModel;
            statusWindow.Show();

            // Run algorithm asynchronously.
            _currentGA = ga;
            new Task(ga.Start).Start();
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
        #endregion

        #region Diagnostics functions
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
        #endregion

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            Canvas c = new Canvas();
            ItemsControl control = new ItemsControl();
            var template = uxDataFloor.ItemTemplate;
            control.ItemTemplate = Application.Current.FindResource("SegmentRowTemplate") as DataTemplate;
            control.ItemsSource = _building.Floors[0].Segments;
            c.Children.Add(control);
            
            c.Measure(new Size(1000, 1000));
            c.Arrange(new Rect(new Size(1000, 1000)));

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)control.DesiredSize.Width, (int)control.DesiredSize.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(control);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (Stream stm = File.Create(@"D:\test.png"))
            {
                encoder.Save(stm);
            }
            return;

            //int width = (int)uxDataFloor.ActualWidth;
            //int height = (int)uxDataFloor.ActualHeight;
            //int width = 1000;
            //int height = 1000;

            //uxDataFloor.Measure(new Size(width, height));
            //uxDataFloor.Arrange(new Rect(uxDataFloor.DesiredSize));

            //RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            //bmp.Render(uxDataFloor);

            //var encoder = new PngBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(bmp));
            //using (Stream stm = File.Create(@"D:\test.png"))
            //{
            //    encoder.Save(stm);
            //}
        }
    }
}

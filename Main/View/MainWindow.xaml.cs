using Genetics;
using Genetics.Operators;
using Genetics.Specialized;
using Main.ViewModel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Main.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        private Calculator _viewModel;
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new Calculator(uxWorkspaceViewbox, this);

            DataContext = _viewModel;
        }

        #region Actions
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
                _viewModel.LoadBuilding(dlg.FileName);
            }
        }

        private void ProcessCancel_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CancelGA();
        }

        private void ProcessRun_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.RunGA();
        }

        private void ToolsEditor_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(typeof(BuildingEditor.App).Assembly.Location, _viewModel.CurrentFile);
        }

        private void ToolsGenetics_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowGeneticsConfiguration();
            
        }

        /// <summary>
        /// Updates view model view mode.
        /// </summary>
        private void ViewMode_Clicked(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            string mode = element.Tag as string;

            if (mode != null)
                _viewModel.SetViewMode(mode);
        }
        
        private void Window_Activated(object sender, EventArgs e)
        {
            _viewModel.RefreshFileIfDirty();
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
            control.ItemsSource = _viewModel.CurrentBuilding.Floors[0].Segments;
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

        private void Test2_Click(object sender, RoutedEventArgs e)
        {
            BinaryChromosome.MutationOperator = new PathDirectionMutation(_viewModel.CurrentBuilding, 1);

            BinaryChromosome c = new BinaryChromosome(_viewModel.CurrentBuilding.GetFenotype().ToGenotype());
            c.Mutate();
            _viewModel.CurrentBuilding.SetFenotype(c.Genotype.ToFenotype());
            _viewModel.CurrentBuilding.DrawSolution();
        }
    }
}

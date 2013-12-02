using Genetics.Specialized;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Main.ViewModel.GeneticsConfiguration
{
    [ImplementPropertyChanged]
    public class MultiPointCrossoverConfiguration : IGUIConfiguration, ICrossoverConfiguration
    {
        public MultiPointCrossoverConfiguration()
        {
            GUI = BuildGUIConfiguration();
            Points = 2;
        }

        public Genetics.Generic.ICrossoverOperator<List<bool>> BuildCrossoverOperator()
        {
            return new MultiPointCrossover(Points);
        }

        public System.Windows.FrameworkElement BuildGUIConfiguration()
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            grid.Children.Add(new Label() { Content = "Points: " });

            TextBox points = new TextBox() { VerticalAlignment = VerticalAlignment.Center };
            points.SetBinding(TextBox.TextProperty, new Binding("Points"));
            grid.Children.Add(points);
            Grid.SetColumn(points, 1);

            return grid;
        }

        public string Name { get { return "Multi point"; } }
        public int Points { get; set; }

        public System.Windows.FrameworkElement GUI { get; set; }
    }
}

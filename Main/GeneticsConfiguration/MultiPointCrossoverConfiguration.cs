using Genetics.Specialized;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Main.GeneticsConfiguration
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
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            panel.Children.Add(new Label() { Content = "Points: " });

            TextBox points = new TextBox() { Width = 30, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            points.SetBinding(TextBox.TextProperty, new Binding("Points"));
            panel.Children.Add(points);

            return panel;
        }

        public string Name { get { return "Multi point"; } }
        public int Points { get; set; }

        public System.Windows.FrameworkElement GUI { get; set; }
    }
}

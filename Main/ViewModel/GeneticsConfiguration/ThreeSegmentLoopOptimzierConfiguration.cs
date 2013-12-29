using Genetics.Operators;
using Genetics.Specialized;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Main.ViewModel.GeneticsConfiguration
{
    [ImplementPropertyChanged]
    public class ThreeSegmentLoopOptimizerConfiguration : IGUIConfiguration, ITransformationConfiguration
    {
        public ThreeSegmentLoopOptimizerConfiguration()
        {
            GUI = BuildGUIConfiguration();

            // Default values
            Probability = 0.01;
        }

        public System.Windows.FrameworkElement BuildGUIConfiguration()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            panel.Children.Add(new Label() { Content = "Probability: " });

            TextBox probability = new TextBox() { Width = 30, VerticalAlignment = System.Windows.VerticalAlignment.Center };
            probability.SetBinding(TextBox.TextProperty, new Binding("Probability"));
            panel.Children.Add(probability);

            return panel;
        }

        public string Name { get { return "3-seg. loop optimizer"; } }
        public double Probability { get; set; }

        public System.Windows.FrameworkElement GUI { get; set; }
    
#region ITransformationConfiguration Members

public Genetics.Generic.ITransformer<List<bool>> BuildTransformer(BuildingEditor.ViewModel.Building building)
{
    return new ThreeSegmentLoopOptimizer(building, Probability);
}

#endregion
}
}

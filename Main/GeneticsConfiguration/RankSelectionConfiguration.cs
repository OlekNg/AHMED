using Genetics.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Main.GeneticsConfiguration
{
    public class RankSelectionConfiguration : IGUIConfiguration, ISelectionConfiguration
    {
        public RankSelectionConfiguration()
        {
            GUI = BuildGUIConfiguration();
        }

        public Genetics.Generic.ISelector BuildSelector()
        {
            return new RankSelector();
        }

        public System.Windows.FrameworkElement BuildGUIConfiguration()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            panel.Children.Add(new Label() { Content = "Mode: " });

            ComboBox combo = new ComboBox();
            combo.Items.Add("Hello");
            combo.Items.Add("World");
            panel.Children.Add(combo);

            return panel;
        }

        public string Name
        {
            get { return "Rank selection"; }
        }

        public System.Windows.FrameworkElement GUI { get; set; }
        public System.Windows.FrameworkElement PercentageGUI { get; set; }
        public System.Windows.FrameworkElement NumberGUI { get; set; }
    }
}

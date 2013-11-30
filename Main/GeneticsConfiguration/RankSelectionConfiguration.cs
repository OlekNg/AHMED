using Genetics.Specialized;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Main.GeneticsConfiguration
{
    [ImplementPropertyChanged]
    public class RankSelectionConfiguration : IGUIConfiguration, ISelectionConfiguration
    {
        public RankSelectionConfiguration()
        {
            GUI = BuildGUIConfiguration();

            // Default values
            Percentage = 0.5;
            Number = 10;
        }

        public Genetics.Generic.ISelector BuildSelector()
        {
            return new RankSelector();
        }

        public FrameworkElement BuildGUIConfiguration()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            panel.Children.Add(new Label() { Content = "Mode: " });

            // Combobox
            ComboBox combo = new ComboBox();
            combo.SetBinding(ComboBox.SelectedItemProperty, new Binding("SelectedMode"));

            // Combo percentage mode
            Mode percentageMode = new Mode() { Name = "Percentage" };
            StackPanel modePanel = new StackPanel() { Orientation = Orientation.Horizontal };
            modePanel.Children.Add(new Label() { Content = "Percentage:" });
            TextBox tb = new TextBox();
            tb.SetBinding(TextBox.TextProperty, new Binding("Percentage"));
            modePanel.Children.Add(tb);
            percentageMode.ModeGUI = modePanel;

            // Number mode
            Mode numberMode = new Mode() { Name = "Best N" };
            modePanel = new StackPanel() { Orientation = Orientation.Horizontal };
            modePanel.Children.Add(new Label() { Content = "N:" });
            tb = new TextBox();
            tb.SetBinding(TextBox.TextProperty, new Binding("Number"));
            modePanel.Children.Add(tb);
            numberMode.ModeGUI = modePanel;

            combo.ItemsSource = new List<Mode>() { percentageMode, numberMode };
            panel.Children.Add(combo);

            
            ContentControl c = new ContentControl();
            c.SetBinding(ContentControl.ContentProperty, new Binding("SelectedMode.ModeGUI"));

            StackPanel mainPanel = new StackPanel();
            mainPanel.Children.Add(panel);
            mainPanel.Children.Add(c);

            return mainPanel;
        }

        public string Name { get { return "Rank selection"; } }
        public double Percentage { get; set; }
        public int Number { get; set; }

        public FrameworkElement GUI { get; set; }
        public Mode SelectedMode { get; set; }

        public class Mode
        {
            public FrameworkElement ModeGUI { get; set; }

            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}

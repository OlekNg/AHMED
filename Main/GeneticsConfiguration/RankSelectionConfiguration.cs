using Genetics.Specialized;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            // Default values
            Percentage = 0.5;
            Number = 10;

            GUI = BuildGUIConfiguration();
        }

        public Genetics.Generic.ISelector BuildSelector()
        {
            return new RankSelector(SelectedMode.Mode) { Number = Number, Percentage = Percentage };
        }

        public FrameworkElement BuildGUIConfiguration()
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Label l = new Label() { Content = "Mode: " };
            grid.Children.Add(l);
            Grid.SetRow(l, 0);
            Grid.SetColumn(l, 0);

            // Combobox
            ComboBox combo = new ComboBox() { VerticalAlignment = VerticalAlignment.Center };
            combo.SetBinding(ComboBox.SelectedItemProperty, new Binding("SelectedMode"));

            // Combo percentage mode
            Grid modeGrid = new Grid();
            modeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            modeGrid.ColumnDefinitions.Add(new ColumnDefinition());

            Mode percentageMode = new Mode() { Name = "Percentage", Mode = RankSelector.SelectionMode.Percentage };

            l = new Label() { Content = "Percentage:" };
            modeGrid.Children.Add(l);
            Grid.SetColumn(l, 0);

            TextBox tb = new TextBox() { VerticalAlignment = VerticalAlignment.Center };
            tb.SetBinding(TextBox.TextProperty, new Binding("Percentage"));
            modeGrid.Children.Add(tb);
            Grid.SetColumn(tb, 1);

            percentageMode.ModeGUI = modeGrid;

            // Number mode
            modeGrid = new Grid();
            modeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            modeGrid.ColumnDefinitions.Add(new ColumnDefinition());

            Mode numberMode = new Mode() { Name = "Best N", Mode = RankSelector.SelectionMode.Number };

            l = new Label() { Content = "N:" };
            modeGrid.Children.Add(l);
            Grid.SetColumn(l, 0);

            tb = new TextBox() { VerticalAlignment = VerticalAlignment.Center };
            tb.SetBinding(TextBox.TextProperty, new Binding("Number"));
            modeGrid.Children.Add(tb);
            Grid.SetColumn(tb, 1);

            numberMode.ModeGUI = modeGrid;

            combo.ItemsSource = new ObservableCollection<Mode>() { percentageMode, numberMode };
            
            grid.Children.Add(combo);
            Grid.SetRow(combo, 0);
            Grid.SetColumn(combo, 1);
            
            ContentControl c = new ContentControl();
            c.SetBinding(ContentControl.ContentProperty, new Binding("SelectedMode.ModeGUI"));

            grid.Children.Add(c);
            Grid.SetRow(c, 1);
            Grid.SetColumn(c, 0);
            Grid.SetColumnSpan(c, 2);

            return grid;
        }

        public string Name { get { return "Rank selection"; } }
        public double Percentage { get; set; }
        public int Number { get; set; }

        public FrameworkElement GUI { get; set; }
        public Mode SelectedMode { get; set; }

        [ImplementPropertyChanged]
        public class Mode
        {
            public FrameworkElement ModeGUI { get; set; }

            public string Name { get; set; }
            public RankSelector.SelectionMode Mode { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}

using BuildingEditor.Logic;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace BuildingEditor.Tools.Logic
{
    [ImplementPropertyChanged]
    public class PeopleTool : Tool
    {
        private Building _building;


        public PeopleTool(Building building)
        {
            _building = building;
            Name = "People";
            GroupCount = 3;
        }

        public int GroupCount { get; set; }

        protected override System.Windows.FrameworkElement BuildGUIConfiguration()
        {
            TextBox capacity = new TextBox() { Width = 20, Height = 20 };
            capacity.SetBinding(TextBox.TextProperty, new Binding("GroupCount"));

            StackPanel capacityPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            capacityPanel.Children.Add(new Label() { Content = "Count: " });
            capacityPanel.Children.Add(capacity);

            return capacityPanel;
        }

        public override void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var segment = SenderToSegment(sender);
            segment.PeopleCount = GroupCount;
        }

        public override void MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                GroupCount++;
            else if (GroupCount > 0)
                GroupCount--;
        }
    }
}

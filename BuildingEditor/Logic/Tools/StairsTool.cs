using BuildingEditor.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BuildingEditor.Tools.Logic
{
    public class StairsTool : Tool
    {
        private Building _building;
        private Segment _previewSegment;

        public StairsTool(Building building)
        {
            _building = building;
            Name = "Stairs";
            Capacity = 3;
            Delay = 2;
        }

        public int Capacity { get; set; }
        public int Delay { get; set; }

        protected override FrameworkElement BuildGUIConfiguration()
        {
            TextBox capacity = new TextBox() { Width = 20, Height = 20 };
            capacity.SetBinding(TextBox.TextProperty, new Binding("Capacity"));

            StackPanel capacityPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            capacityPanel.Children.Add(new Label() { Content = "Capacity" });
            capacityPanel.Children.Add(capacity);

            TextBox delay = new TextBox() { Width = 20, Height = 20 };
            delay.SetBinding(TextBox.TextProperty, new Binding("Delay"));

            StackPanel delayPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            delayPanel.Children.Add(new Label() { Content = "Delay" });
            delayPanel.Children.Add(delay);

            StackPanel panel = new StackPanel();
            panel.Children.Add(capacityPanel);
            panel.Children.Add(delayPanel);

            return panel;
        }

        public override void CancelAction()
        {
            if (_previewSegment != null)
            {
                _previewSegment.Preview = false;
                _previewSegment = null;
            }
        }

        public override void MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SegmentSide segmentSide = ProcessEventArg(sender, e);
            var segment = segmentSide.Segment;

            if (_previewSegment != null)
                _previewSegment.Preview = false;

            _previewSegment = segment;
            segment.Preview = true;
            segment.PreviewType = SegmentType.STAIRS;
            segment.PreviewOrientation = segmentSide.Side;
        }

        public override void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SegmentSide segmentSide = ProcessEventArg(sender, e);

            var segment = segmentSide.Segment;
            segment.Type = (segment.Type == SegmentType.STAIRS ? SegmentType.NONE : SegmentType.STAIRS);
            segment.Orientation = segmentSide.Side;

            _building.CurrentFloor.UpdateRender();
        }
    }
}

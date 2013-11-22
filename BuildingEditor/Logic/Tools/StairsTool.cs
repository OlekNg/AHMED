using BuildingEditor.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<StairsPair> _stairs;
        private Segment _previewSegment;
        private bool _firstStairs;
        private StairsPair _stairsPair;

        public StairsTool(Building building)
        {
            _building = building;
            _stairs = _building.Stairs;
            _firstStairs = true;
            Name = "Stairs";
            Capacity = 3;
            Delay = 2;
            UpdateMessage();
        }

        public int Capacity { get; set; }
        public int Delay { get; set; }
        public string Message { get; set; }

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

            Label messageLabel = new Label();
            messageLabel.SetBinding(Label.ContentProperty, new Binding("Message"));

            StackPanel panel = new StackPanel();
            panel.Children.Add(capacityPanel);
            panel.Children.Add(delayPanel);
            panel.Children.Add(messageLabel);

            return panel;
        }

        public override void CancelAction()
        {
            // To abort action while waiting for second stairs,
            // we have to remove first stairs.
            if (!_firstStairs)
            {
                _stairsPair.First.AssignedSegment.Type = SegmentType.FLOOR;
            }

            _firstStairs = true;
            UpdateMessage();
        }

        public override void ClearPreview()
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

            // Do not override another stairs.
            if (segment.Type == SegmentType.STAIRS) return;

            segment.Type = (segment.Type == SegmentType.STAIRS ? SegmentType.NONE : SegmentType.STAIRS);
            segment.Orientation = segmentSide.Side;

            if (_firstStairs)
            {
                _stairsPair = new StairsPair(_stairs);
                _stairsPair.First = new Stairs()
                {
                    AssignedSegment = segment,
                    Capacity = Capacity,
                    Delay = Delay,
                    Level = _building.CurrentFloor.Level
                };
            }
            else
            {
                _stairsPair.Second = new Stairs()
                {
                    AssignedSegment = segment,
                    Capacity = Capacity,
                    Delay = Delay,
                    Level = _building.CurrentFloor.Level
                };

                _stairsPair.SetAdditionalData();
                _stairs.Add(_stairsPair);
            }

            _firstStairs = !_firstStairs;
            UpdateMessage();

            _building.CurrentFloor.UpdateRender();
        }

        private void UpdateMessage()
        {
            Message = _firstStairs == true ? "Set first stairs." : "Set second stairs";
        }
    }
}

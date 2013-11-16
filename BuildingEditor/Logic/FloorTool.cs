using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

namespace WPFTest.Logic
{
    [ImplementPropertyChanged]
    public class FloorTool : Tool
    {
        private Building _building;
        private Segment _selectionStart;
        private Segment _selectionEnd;
        private List<Segment> _selectedSegments;

        public FloorTool(Building b)
        {
            _building = b;
            _selectedSegments = new List<Segment>();
            Name = "Floor";
        }

        public int Capacity { get; set; }
        public bool ClearMode { get; set; }

        public override void CancelAction()
        {
            if (_selectionStart != null)
            {
                _selectionStart = _selectionEnd = null;
                UpdateSelectionPreview();
            }
        }

        #region Mouse event handlers
        public override void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Segment segment = SenderToSegment(sender);
            _selectionEnd = _selectionStart = segment;

            UpdateSelectionPreview();
        }

        public override void MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_selectionStart != null)
            {
                _selectionEnd = SenderToSegment(sender);
                UpdateSelectionPreview();
            }
            else
                SenderToSegment(sender).Preview = true;
        }

        public override void MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _selectionStart = _selectionEnd = null;
            Apply();
            UpdateSelectionPreview();
        }


        public override void MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_selectionStart != null) return;

            Segment segment = SenderToSegment(sender);
            segment.Preview = true;
        }

        public override void MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_selectionStart != null) return;

            Segment segment = SenderToSegment(sender);
            segment.Preview = false;
        }
        #endregion

        protected override FrameworkElement BuildConfiguration()
        {
            CheckBox clearMode = new CheckBox() { Content = "Clear mode" };
            clearMode.SetBinding(CheckBox.IsCheckedProperty, new Binding("ClearMode"));

            TextBox capacity = new TextBox() { Width = 20, Height = 20 };
            capacity.SetBinding(TextBox.TextProperty, new Binding("Capacity"));

            StackPanel capacityPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            capacityPanel.Children.Add(new Label() { Content = "Capacity" });
            capacityPanel.Children.Add(capacity);

            StackPanel panel = new StackPanel();
            panel.Children.Add(clearMode);
            panel.Children.Add(capacityPanel);

            return panel;
        }

        private void Apply()
        {
            SegmentType value = ClearMode == true ?  SegmentType.NONE : SegmentType.FLOOR;
            _selectedSegments.ForEach(x => { x.Type = value; x.Capacity = Capacity; });
            _building.CurrentFloor.UpdateRender();
        }

        private void UpdateSelectionPreview()
        {
            List<Segment> oldSelection = _selectedSegments;
            _selectedSegments = CalcualateAffectedSegments();
            oldSelection.Except(_selectedSegments).ToList().ForEach(x => x.Preview = false);
            _selectedSegments.ForEach(x => x.Preview = true);
        }

        private List<Segment> CalcualateAffectedSegments()
        {
            List<Segment> result = new List<Segment>();

            if (_selectionStart == null || _selectionEnd == null)
                return result;

            int rowBegin, rowEnd, colBegin, colEnd;
            rowBegin = Math.Min(_selectionStart.Row, _selectionEnd.Row);
            rowEnd = Math.Max(_selectionStart.Row, _selectionEnd.Row);
            colBegin = Math.Min(_selectionStart.Column, _selectionEnd.Column);
            colEnd = Math.Max(_selectionStart.Column, _selectionEnd.Column);

            for (int row = rowBegin; row <= rowEnd; row++)
                for (int col = colBegin; col <= colEnd; col++)
                    result.Add(_building.CurrentFloor.Data[row][col]);

            return result;
        }
    }
}

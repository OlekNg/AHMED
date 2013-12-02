using BuildingEditor.ViewModel;
using Common.DataModel.Enums;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace BuildingEditor.ViewModel.Tools
{
    [ImplementPropertyChanged]
    public class DeleteTool : Tool
    {
        private List<Segment> _selectedSegments;
        private Segment _mouseoverSegment;
        private Editor _editor;

        public DeleteTool(Editor editor)
        {
            _editor = editor;
            _selectedSegments = new List<Segment>();
            Name = "Reduce building";
            DeleteRow = true;
        }

        public bool DeleteRow { get; set; }
        public bool DeleteColumn { get; set; }

        protected override System.Windows.FrameworkElement BuildGUIConfiguration()
        {
            RadioButton r1 = new RadioButton()
            {
                Content = "Row",
                GroupName = "DeleteMode",
            };
            r1.SetBinding(RadioButton.IsCheckedProperty, new Binding("DeleteRow"));

            RadioButton r2 = new RadioButton()
            {
                Content = "Column",
                GroupName = "DeleteMode",
            };
            r2.SetBinding(RadioButton.IsCheckedProperty, new Binding("DeleteColumn"));

            StackPanel panel = new StackPanel();
            panel.Children.Add(r1);
            panel.Children.Add(r2);

            return panel;
        }

        public override void ClearPreview()
        {
            _mouseoverSegment = null;
            UpdateSelectionPreview();
        }

        public override void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Apply();
        }

        public override void MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _mouseoverSegment = SenderToSegment(sender);
            UpdateSelectionPreview();
        }

        public override void MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (DeleteRow)
                DeleteColumn = true;
            else
                DeleteRow = true;

            UpdateSelectionPreview();
        }

        private void Apply()
        {
            if (DeleteRow)
                _editor.CurrentBuilding.CurrentFloor.RemoveRow(_mouseoverSegment.Row);
            else
                _editor.CurrentBuilding.CurrentFloor.RemoveColumn(_mouseoverSegment.Column);
        }

        private void UpdateSelectionPreview()
        {
            List<Segment> oldSelection = _selectedSegments;
            _selectedSegments = CalcualateAffectedSegments();
            oldSelection.Except(_selectedSegments).ToList().ForEach(x => x.Preview = false);
            _selectedSegments.ForEach(x => { x.Preview = true; x.PreviewType = SegmentType.NONE; });
        }

        private List<Segment> CalcualateAffectedSegments()
        {
            List<Segment> result = new List<Segment>();

            if (_mouseoverSegment == null)
                return result;

            if (DeleteRow)
            {
                result = _editor.CurrentBuilding.CurrentFloor.Segments[_mouseoverSegment.Row].ToList();
            }
            else
            {
                result = _editor.CurrentBuilding.CurrentFloor.Segments.Select(x => x.Where(y => y.Column == _mouseoverSegment.Column).First()).ToList();
            }

            return result;
        }
    }
}

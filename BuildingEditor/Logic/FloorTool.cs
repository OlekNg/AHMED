using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Shapes;

namespace WPFTest.Logic
{
    public class FloorTool : Tool
    {
        private Building _building;
        private Segment _selectionStart;
        private Segment _selectionEnd;
        private List<Segment> _selectedSegments;

        public FloorTool(Building b)
        {
            _building = b;
            Name = "Floor";
            _selectedSegments = new List<Segment>();
        }

        public override void CancelAction()
        {
            if (_selectionStart != null)
            {
                _selectionStart = _selectionEnd = null;
                UpdateSelectionPreview();
            }
        }

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
        }

        public override void MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _selectionStart = _selectionEnd = null;
            UpdateSelectionPreview();
        }



        public override void MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Segment segment = SenderToSegment(sender);

            //segment.Preview = true;
        }

        public override void MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Segment segment = SenderToSegment(sender);

            //segment.Preview = false;
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

            if (_selectionStart.Row < _selectionEnd.Row)
            {
                rowBegin = _selectionStart.Row;
                rowEnd = _selectionEnd.Row;
            }
            else
            {
                rowBegin = _selectionEnd.Row;
                rowEnd = _selectionStart.Row;
            }

            if (_selectionStart.Column < _selectionEnd.Column)
            {
                colBegin = _selectionStart.Column;
                colEnd = _selectionEnd.Column;
            }
            else
            {
                colBegin = _selectionEnd.Column;
                colEnd = _selectionStart.Column;
            }

            for (int row = rowBegin; row <= rowEnd; row++)
                for (int col = colBegin; col <= colEnd; col++)
                    result.Add(_building.Data[row][col]);

            return result;
        }
    }
}

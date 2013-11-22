using BuildingEditor.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

namespace BuildingEditor.Tools.Logic
{
    public class SegmentSide
    {
        public Segment Segment { get; set; }
        public Side Side { get; set; }
        public Point MousePosition { get; set; }
        public SideElement SideElement { get { return Segment.GetSideElement(Side); } }

        public void Set(SideElementType value)
        {
            Segment.SetSide(Side, value);
        }
    }

    public class SideElementTool : Tool
    {
        protected Building _building;
        protected SegmentSide _selectionStart;
        protected SegmentSide _selectionEnd;
        protected List<SideElement> _selectedSides = new List<SideElement>();
        protected SideElementType _elementType;

        public SideElementTool(Building b, SideElementType elementType, string name = "SideTool")
        {
            _building = b;
            _elementType = elementType;
            Name = name;
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
        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {
            SegmentSide segmentSide = ProcessEventArg(sender, e);
            if (segmentSide == null) return;

            _selectionStart = _selectionEnd = segmentSide;
            UpdateSelectionPreview();
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            SegmentSide segmentSide = ProcessEventArg(sender, e);
            if (segmentSide == null) return;

            if (_selectionStart != null && e.LeftButton == MouseButtonState.Pressed)
            {
                _selectionEnd = segmentSide;
                UpdateSelectionPreview();
            }
            else
            {
                _selectionStart = _selectionEnd = segmentSide;
                UpdateSelectionPreview();
            }
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            _selectionStart = _selectionEnd = null;
            Apply();
            UpdateSelectionPreview();
        }
        #endregion

        private void Apply()
        {
            SideElementType value = ClearMode == true ? SideElementType.NONE : _elementType;
            _selectedSides.ForEach(x => { x.Type = value; x.Capacity = Capacity; });
            _building.CurrentFloor.UpdateRender();
        }

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
        
        /// <summary>
        /// Calculates new affected walls by selection and applies
        /// difference between actual selection and selection before actual.
        /// </summary>
        private void UpdateSelectionPreview()
        {
            List<SideElement> oldSelection = _selectedSides;
            _selectedSides = CalcualateAffectedSides();
            oldSelection.Except(_selectedSides).ToList().ForEach(x => x.Preview = false);
            _selectedSides.ForEach(x => { x.PreviewType = _elementType; x.Preview = true; });
        }

        /// <summary>
        /// Calculates affected wall basing on selection start and selection end.
        /// </summary>
        /// <returns>List of side elements that are affected by selection.</returns>
        private List<SideElement> CalcualateAffectedSides()
        {
            List<SideElement> result = new List<SideElement>();

            if (_selectionStart == null || _selectionEnd == null)
                return result;

            int rowBegin, rowEnd, colBegin, colEnd;
            rowBegin = Math.Min(_selectionStart.Segment.Row, _selectionEnd.Segment.Row);
            rowEnd = Math.Max(_selectionStart.Segment.Row, _selectionEnd.Segment.Row);
            colBegin = Math.Min(_selectionStart.Segment.Column, _selectionEnd.Segment.Column);
            colEnd = Math.Max(_selectionStart.Segment.Column, _selectionEnd.Segment.Column);

            if (rowBegin == rowEnd && colBegin == colEnd)
            {
                result.Add(_selectionEnd.SideElement);
                return result;
            }

            if (Math.Abs(rowEnd - rowBegin) > Math.Abs(colEnd - colBegin))
            {
                // Vertical line
                Side s = GetHorizontalRelation();
                for (int row = rowBegin; row <= rowEnd; row++)
                    result.Add(_building.CurrentFloor.Data[row][_selectionStart.Segment.Column].GetSideElement(s));
            }
            else
            {
                // Horizontal line
                Side s = GetVerticalRelation();
                for (int col = colBegin; col <= colEnd; col++)
                    result.Add(_building.CurrentFloor.Data[_selectionStart.Segment.Row][col].GetSideElement(s));

            }

            return result;
        }

        private Side GetVerticalRelation()
        {
            Point pos = _selectionEnd.MousePosition;
            int startRow = _selectionStart.Segment.Row;
            int endRow = _selectionEnd.Segment.Row;

            if (startRow == endRow)
                return pos.Y < 0 ? Side.TOP : Side.BOTTOM;
            else
                return startRow > endRow ? Side.TOP : Side.BOTTOM;
        }

        private Side GetHorizontalRelation()
        {
            Point pos = _selectionEnd.MousePosition;
            int startCol = _selectionStart.Segment.Column;
            int endCol = _selectionEnd.Segment.Column;

            if (startCol == endCol)
                return pos.X < 0 ? Side.LEFT : Side.RIGHT;
            else
                return startCol > endCol ? Side.LEFT : Side.RIGHT;
        }
    }
}

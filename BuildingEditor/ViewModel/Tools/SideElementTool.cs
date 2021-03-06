﻿using BuildingEditor.ViewModel;
using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

namespace BuildingEditor.ViewModel.Tools
{
    public class SegmentSide
    {
        public Segment Segment { get; set; }
        public Direction Side { get; set; }
        public Point MousePosition { get; set; }
        public SideElement SideElement { get { return Segment.GetSideElement(Side); } }

        public void Set(SideElementType value)
        {
            Segment.SetSide(Side, value);
        }
    }

    public class SideElementTool : Tool
    {
        protected SegmentSide _selectionStart;
        protected SegmentSide _selectionEnd;
        protected List<SideElement> _selectedSides = new List<SideElement>();
        protected SideElementType _elementType;
        private Editor _editor;
        private bool _enableCapacity;

        public SideElementTool(Editor editor, SideElementType elementType, string name = "SideTool", bool enableCapacity = false)
        {
            _editor = editor;
            _elementType = elementType;
            _enableCapacity = enableCapacity;
            Name = name;
            Capacity = 3;
            GUIConfiguration = BuildGUIConfiguration();
        }

        public int Capacity { get; set; }
        public bool ClearMode { get; set; }

        private SideElementType _previewType { get { return ClearMode == true ? SideElementType.NONE : _elementType; } }

        public override void ClearPreview()
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

        public override void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                Capacity++;
            else if (Capacity > 1)
                Capacity--;
        }
        #endregion

        private void Apply()
        {
            SideElementType value = ClearMode == true ? SideElementType.NONE : _elementType;
            _selectedSides.ForEach(x => { x.Type = value; x.Capacity = Capacity; });
            _editor.CurrentBuilding.CurrentFloor.UpdateRender();
        }

        protected override FrameworkElement BuildGUIConfiguration()
        {
            StackPanel panel = new StackPanel();

            CheckBox clearMode = new CheckBox() { Content = "Clear mode" };
            clearMode.SetBinding(CheckBox.IsCheckedProperty, new Binding("ClearMode"));
            panel.Children.Add(clearMode);

            if (_enableCapacity)
            {
                TextBox capacity = new TextBox() { Width = 20, Height = 20 };
                capacity.SetBinding(TextBox.TextProperty, new Binding("Capacity"));

                StackPanel capacityPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                capacityPanel.Children.Add(new Label() { Content = "Capacity" });
                capacityPanel.Children.Add(capacity);
                panel.Children.Add(capacityPanel);
            }

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
            _selectedSides.ForEach(x => { x.PreviewType = _previewType; x.Preview = true; });
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
                Direction s = GetHorizontalRelation();
                for (int row = rowBegin; row <= rowEnd; row++)
                    result.Add(_editor.CurrentBuilding.CurrentFloor.Segments[row][_selectionStart.Segment.Column].GetSideElement(s));
            }
            else
            {
                // Horizontal line
                Direction s = GetVerticalRelation();
                for (int col = colBegin; col <= colEnd; col++)
                    result.Add(_editor.CurrentBuilding.CurrentFloor.Segments[_selectionStart.Segment.Row][col].GetSideElement(s));

            }

            return result;
        }

        private Direction GetVerticalRelation()
        {
            Point pos = _selectionEnd.MousePosition;
            int startRow = _selectionStart.Segment.Row;
            int endRow = _selectionEnd.Segment.Row;

            if (startRow == endRow)
                return pos.Y < 0 ? Direction.UP : Direction.DOWN;
            else
                return startRow > endRow ? Direction.UP : Direction.DOWN;
        }

        private Direction GetHorizontalRelation()
        {
            Point pos = _selectionEnd.MousePosition;
            int startCol = _selectionStart.Segment.Column;
            int endCol = _selectionEnd.Segment.Column;

            if (startCol == endCol)
                return pos.X < 0 ? Direction.LEFT : Direction.RIGHT;
            else
                return startCol > endCol ? Direction.LEFT : Direction.RIGHT;
        }
    }
}

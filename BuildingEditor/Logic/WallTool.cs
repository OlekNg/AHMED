﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace WPFTest.Logic
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

    public class WallTool : Tool
    {
        private Building _building;
        private SegmentSide _selectionStart;
        private SegmentSide _selectionEnd;
        private List<SideElement> _selectedWalls = new List<SideElement>();

        public WallTool(Building b)
        {
            _building = b;
            Name = "Wall";
        }

        public override void CancelAction()
        {
            if (_selectionStart != null)
            {
                _selectionStart = _selectionEnd = null;
                UpdateSelectionPreview();
            }
        }

        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {
            SegmentSide segmentSide = ProcessEventArg(sender, e);
            if (segmentSide == null) return;

            _selectionStart = _selectionEnd = segmentSide;
            UpdateSelectionPreview();
        }

        

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (_selectionStart != null)
            {
                SegmentSide segmentSide = ProcessEventArg(sender, e);
                if (segmentSide == null) return;

                _selectionEnd = segmentSide;
                UpdateSelectionPreview();
            }
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            _selectionStart = _selectionEnd = null;
            UpdateSelectionPreview();
        }

        public override void MouseEnter(object sender, MouseEventArgs e)
        {
        }

        public override void MouseLeave(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Caluclates which side (triangle) of segment has been clicked.
        /// </summary>
        /// <param name="size">Size of segment (rendered width/height - assumption that segment is square).</param>
        /// <param name="pos">Mouse click location relative to segment.</param>
        /// <returns></returns>
        private Side CalculateSegmentSide(double size, Point pos)
        {
            if (pos.X > pos.Y && pos.X < (size - pos.Y))
                return Side.TOP;

            if (pos.X > pos.Y && pos.X > (size - pos.Y))
                return Side.RIGHT;

            if (pos.X < pos.Y && pos.X < (size - pos.Y))
                return Side.LEFT;

            if (pos.X < pos.Y && pos.X > (size - pos.Y))
                return Side.BOTTOM;

            return Side.BOTTOM;
        }

        private SegmentSide ProcessEventArg(object sender, MouseEventArgs e)
        {
            Rectangle shape = sender as Rectangle;
            if (shape == null) return null;

            Segment segment = shape.Tag as Segment;
            if (segment == null) return null;

            Point pos = e.GetPosition((UIElement)sender);
            var size = shape.ActualHeight;

            Side side = CalculateSegmentSide(size, pos);

            pos.X -= (size / 2);
            pos.Y -= (size / 2);

            return new SegmentSide { Segment = segment, Side = side, MousePosition = pos };
        }

        /// <summary>
        /// Calculates new affected walls by selection and applies
        /// difference between actual selection and selection before actual.
        /// </summary>
        private void UpdateSelectionPreview()
        {
            List<SideElement> oldSelection = _selectedWalls;
            _selectedWalls = CalcualateAffectedWalls();
            oldSelection.Except(_selectedWalls).ToList().ForEach(x => x.PreviewWall = false);
            _selectedWalls.ForEach(x => x.PreviewWall = true);
        }

        /// <summary>
        /// Calculates affected wall basing on selection start and selection end.
        /// </summary>
        /// <returns>List of side elements that are affected by selection.</returns>
        private List<SideElement> CalcualateAffectedWalls()
        {
            List<SideElement> result = new List<SideElement>();

            if (_selectionStart == null || _selectionEnd == null)
                return result;

            int rowBegin, rowEnd, colBegin, colEnd;
            rowBegin = Math.Min(_selectionStart.Segment.Row, _selectionEnd.Segment.Row);
            rowEnd = Math.Max(_selectionStart.Segment.Row, _selectionEnd.Segment.Row);
            colBegin = Math.Min(_selectionStart.Segment.Column, _selectionEnd.Segment.Column);
            colEnd = Math.Max(_selectionStart.Segment.Column, _selectionEnd.Segment.Column);

            if (Math.Abs(rowEnd - rowBegin) > Math.Abs(colEnd - colBegin))
            {
                // Vertical line
                Side s = GetHorizontalRelation(_selectionEnd.MousePosition);
                for (int row = rowBegin; row <= rowEnd; row++)
                    result.Add(_building.Data[row][colBegin].GetSideElement(s));
            }
            else
            {
                // Horizontal line
                Side s = GetVerticalRelation(_selectionEnd.MousePosition);
                for (int col = colBegin; col <= colEnd; col++)
                    result.Add(_building.Data[rowBegin][col].GetSideElement(s));

            }

            return result;
        }

        private Side GetVerticalRelation(Point pos)
        {
            return pos.Y < 0 ? Side.TOP : Side.BOTTOM;
        }

        private Side GetHorizontalRelation(Point pos)
        {
            return pos.X < 0 ? Side.LEFT : Side.RIGHT;
        }
    }
}

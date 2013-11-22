using BuildingEditor.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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

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
        private Window _rangeWindow;

        public StairsTool(Building building)
        {
            _building = building;
            _rangeWindow = new RangeWindow();
            Name = "Stairs";
        }



        public override void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Segment segment = SenderToSegment(sender);

            _rangeWindow.ShowDialog();

            segment.Type = (segment.Type == SegmentType.STAIRS ? SegmentType.NONE : SegmentType.STAIRS);
            _building.CurrentFloor.UpdateRender();
        }
    }
}

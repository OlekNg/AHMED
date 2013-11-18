using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFTest.Logic
{
    public class StairsTool : Tool
    {
        private Building _building;

        public StairsTool(Building building)
        {
            _building = building;
            Name = "Stairs";
        }

        public override void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Segment segment = SenderToSegment(sender);
            segment.Type = (segment.Type == SegmentType.STAIRS ? SegmentType.NONE : SegmentType.STAIRS);
            _building.CurrentFloor.UpdateRender();
        }
    }
}

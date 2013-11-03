using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace WPFTest.Logic
{
    public class FloorTool : Tool
    {
        private Building _building;

        public FloorTool(Building b)
        {
            _building = b;
            Name = "Floor";
        }

        public override void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Rectangle shape = sender as Rectangle;
            if (shape == null) return;

            Segment s = shape.Tag as Segment;
            if (s == null) return;

            ToggleFloor(s);
        }

        public override void MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        public override void MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void ToggleFloor(Segment s)
        {
            if (s.Type == SegmentType.FLOOR)
                s.Type = SegmentType.NONE;
            else
                s.Type = SegmentType.FLOOR;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace WPFTest.Logic
{
    public class WallTool : Tool
    {
        private Building _building;

        public WallTool(Building b)
        {
            _building = b;
            Name = "Wall";
        }

        public override void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Rectangle shape = sender as Rectangle;
            if (shape == null) return;

            Segment s = shape.Tag as Segment;
            if (s == null) return;

            Point p = e.GetPosition((UIElement)sender);
            var size = shape.ActualHeight;

            if (p.X > p.Y && p.X < (size - p.Y))
                ToggleWall(s.TopSide);

            if (p.X > p.Y && p.X > (size - p.Y))
                ToggleWall(s.RightSide);

            if (p.X < p.Y && p.X < (size - p.Y))
                ToggleWall(s.LeftSide);

            if (p.X < p.Y && p.X > (size - p.Y))
                ToggleWall(s.BottomSide);
        }

        public override void MouseMove(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ToggleWall(SideElement e)
        {
            if (e.Type == SideElementType.WALL)
                e.Type = SideElementType.NONE;
            else
                e.Type = SideElementType.WALL;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace WPFTest.Logic
{
    public enum Side {NONE, LEFT, TOP, RIGHT, BOTTOM }

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

            Side side = GetSide(size, p);

            switch (side)
            {
                case Side.LEFT:
                    ToggleWall(s.LeftSide);
                    break;
                case Side.TOP:
                    ToggleWall(s.TopSide);
                    break;
                case Side.RIGHT:
                    ToggleWall(s.RightSide);
                    break;
                case Side.BOTTOM:
                    ToggleWall(s.BottomSide);
                    break;
            }
            
        }

        public override void MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }

        public override void MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        private void ToggleWall(SideElement e)
        {
            if (e.Type == SideElementType.WALL)
                e.Type = SideElementType.NONE;
            else
                e.Type = SideElementType.WALL;
        }

        private Side GetSide(double size, Point p)
        {
            if (p.X > p.Y && p.X < (size - p.Y))
                return Side.TOP;

            if (p.X > p.Y && p.X > (size - p.Y))
                return Side.RIGHT;

            if (p.X < p.Y && p.X < (size - p.Y))
                return Side.LEFT;

            if (p.X < p.Y && p.X > (size - p.Y))
                return Side.BOTTOM;

            return Side.NONE;
        }
    }
}

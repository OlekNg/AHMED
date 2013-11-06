using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace WPFTest.Logic
{
    public class PrevievWall
    {
        public Segment Segment { get; set; }
        public Side Side { get; set; }
    }

    public class WallTool : Tool
    {
        private Building _building;
        private List<PrevievWall> _selectedWalls = new List<PrevievWall>();

        public WallTool(Building b)
        {
            _building = b;
            Name = "Wall";
        }

        public override void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Rectangle shape = sender as Rectangle;
            if (shape == null) return;

            Segment segment = shape.Tag as Segment;
            if (segment == null) return;

            Point pos = e.GetPosition((UIElement)sender);
            var size = shape.ActualHeight;

            Side side = CalculateSegmentClickedSide(size, pos);

            segment.ToggleSide(side, SideElementType.WALL);

        }

        public override void MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        public override void MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        public override void MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }

        public override void MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }

        /// <summary>
        /// Caluclates which side (triangle) of segment has been clicked.
        /// </summary>
        /// <param name="size">Size of segment (rendered width/height - assumption that segment is square).</param>
        /// <param name="pos">Mouse click location relative to segment.</param>
        /// <returns></returns>
        private Side CalculateSegmentClickedSide(double size, Point pos)
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



        public override void CancelAction()
        {
            
        }
    }
}

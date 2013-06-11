using Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleVisualizer
{
    public partial class TestForm : Form
    {
        private BuildingMap _bmap;

        private List<Direction> _fenotype;

        private PeopleMap _pmap;

        public TestForm(BuildingMap bmap, PeopleMap pmap, List<Direction> fenotype)
        {
            InitializeComponent();

            _bmap = bmap;
            _fenotype = fenotype;
            _pmap = pmap;
            CreateMap();
        }

        private void CreateMap()
        {
            const int tileSize = 50;
            const int wallSize = 5;


            Bitmap b = new Bitmap((int)_bmap.Width * (tileSize + wallSize) + wallSize,
                                  (int)_bmap.Height * (tileSize + wallSize) + wallSize);

            Graphics g = Graphics.FromImage(b);

            // Background color
            g.Clear(Color.FromArgb(255, 230, 230, 230));

            // Pens
            Pen wallPen = new Pen(Color.FromArgb(255, 0, 0, 255), 5);
            Pen doorPen = new Pen(Color.Yellow, 5);

            // Brushes
            SolidBrush tileBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));

            // Draw tiles and walls/doors
            for (int i = 0; i < (int)_bmap.Height; i++)
            {
                for (int j = 0; j < (int)_bmap.Width; j++)
                {
                    if (_bmap.Floor[i][j] != null)
                        g.FillRectangle(tileBrush,
                                        wallSize + j * (tileSize + wallSize),
                                        wallSize + i * (tileSize + wallSize),
                                        tileSize,
                                        tileSize);

                    foreach (Direction d in Enum.GetValues(typeof(Direction)))
                    {
                        // Prepare line coordinates.
                        Point p1 = new Point();
                        Point p2 = new Point();

                        p1.X = j * (tileSize + wallSize) + (int)Math.Floor(wallSize / 2.0);
                        p1.Y = i * (tileSize + wallSize) + (int)Math.Floor(wallSize / 2.0);

                        switch (d)
                        {
                                // Horizontal line (UP, DOWN)
                            case Direction.UP:
                                p2.X = p1.X + tileSize + wallSize * 2;
                                p2.Y = p1.Y;
                                break;

                            case Direction.DOWN:
                                p2.X = p1.X + tileSize + wallSize * 2;
                                p2.Y = p1.Y;
                                // Move line down
                                p1.Y += tileSize + wallSize;
                                p2.Y += tileSize + wallSize;
                                break;

                                // Vertical line (LEFT, RIGHT)
                            case Direction.LEFT:
                                p2.X = p1.X;
                                p2.Y = p1.Y + tileSize + wallSize * 2;
                                break;

                            case Direction.RIGHT:
                                p2.X = p1.X;
                                p2.Y = p1.Y + tileSize + wallSize * 2;
                                // Move line right
                                p1.X += tileSize + wallSize;
                                p2.X += tileSize + wallSize;
                                break;

                            default:
                                break;
                        }

                        if (_bmap.Floor[i][j].GetSide(d).CanPassThrough)
                            // Draw doors or nothing
                            g.DrawLine(doorPen, p1, p2);
                        else
                            // Draw wall
                            g.DrawLine(wallPen, p1, p2);
                    }
                }
            }

                uxImage.Image = b;
        }
    }
}

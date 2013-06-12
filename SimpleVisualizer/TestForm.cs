using Readers;
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

        /// <summary>
        /// Preview building constructor.
        /// </summary>
        /// <param name="bmap"></param>
        /// <param name="pmap"></param>
        public TestForm(BuildingMap bmap, PeopleMap pmap)
        {
            InitializeComponent();
            _bmap = bmap;
            _pmap = pmap;
            PreviewMap();
        }

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


            Bitmap b = new Bitmap((int)_bmap.Width * (tileSize + wallSize) + wallSize + 20,
                                  (int)_bmap.Height * (tileSize + wallSize) + wallSize + 20);

            Graphics g = Graphics.FromImage(b);

            // Background color
            g.Clear(Color.FromArgb(255, 230, 230, 230));

            // Pens
            Pen wallPen = new Pen(Color.FromArgb(255, 0, 0, 255), wallSize);
            Pen doorPen = new Pen(Color.Orange, wallSize);
            Pen pathPen = new Pen(Color.Green, wallSize);

            // Brushes
            SolidBrush tileBrush = new SolidBrush(Color.FromArgb(255, 200, 200, 200));
            SolidBrush peopleStringBrush = new SolidBrush(Color.LimeGreen);
            SolidBrush doorStringBrush = new SolidBrush(Color.Black);

            // Fonts
            Font peopleFont = new Font(FontFamily.GenericSansSerif, 25);
            Font doorFont = new Font(FontFamily.GenericSansSerif, 10);


            // Draw tiles and walls/doors
            for (int i = 0; i < (int)_bmap.Height; i++)
            {
                for (int j = 0; j < (int)_bmap.Width; j++)
                {
                    if (_bmap.Floor[i][j] == null)
                        continue;
                    // Tile
                    g.FillRectangle(tileBrush,
                                    wallSize + j * (tileSize + wallSize),
                                    wallSize + i * (tileSize + wallSize),
                                    tileSize,
                                    tileSize);

                    // Doors/walls
                    foreach (Direction d in Enum.GetValues(typeof(Direction)))
                    {
                        // Nothing to draw.
                        if (!_bmap.Floor[i][j].GetSide(d).Draw)
                            continue;

                        // Prepare line coordinates.
                        Point p1 = new Point();
                        Point p2 = new Point();

                        p1.X = j * (tileSize + wallSize);
                        p1.Y = i * (tileSize + wallSize);

                        switch (d)
                        {
                                // Horizontal line (UP, DOWN)
                            case Direction.UP:
                                p1.Y += (int)Math.Floor(wallSize / 2.0);
                                p2.X = p1.X + tileSize + wallSize * 2;
                                p2.Y = p1.Y;
                                break;

                            case Direction.DOWN:
                                p1.Y += (int)Math.Floor(wallSize / 2.0);
                                p2.X = p1.X + tileSize + wallSize * 2;
                                p2.Y = p1.Y;
                                // Move line down
                                p1.Y += tileSize + wallSize;
                                p2.Y += tileSize + wallSize;
                                break;

                                // Vertical line (LEFT, RIGHT)
                            case Direction.LEFT:
                                p1.X += (int)Math.Floor(wallSize / 2.0);
                                p2.X = p1.X;
                                p2.Y = p1.Y + tileSize + wallSize * 2;
                                break;

                            case Direction.RIGHT:
                                p1.X += (int)Math.Floor(wallSize / 2.0);
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
                        {
                            // Draw doors
                            // Doors has to be shorter.
                            if (p1.Y == p2.Y)
                            {
                                // Horizontal line
                                p1.X += wallSize;
                                p2.X -= wallSize;
                            }
                            else
                            {
                                // Vertical line
                                p1.Y += wallSize;
                                p2.Y -= wallSize;
                            }

                            g.DrawLine(doorPen, p1, p2);
                            PointF pf = new PointF(p1.X, p1.Y);
                            g.DrawString(_bmap.Floor[i][j].GetSide(d).Capacity.ToString(), doorFont, doorStringBrush, pf);
                        }
                        else
                            // Draw wall
                            g.DrawLine(wallPen, p1, p2);
                    }
                }
            }

            // Draw path from each people group
            Point startPoint = new Point();
            Point endPoint = new Point();
            Point endPointDraw = new Point();
            foreach (PeopleGroup group in _pmap.People)
            {
                int i = (int)group.Row;
                int j = (int)group.Col;
                // Start point of the escape path-line.
                startPoint.X = j * (tileSize + wallSize) + wallSize + tileSize / 2;
                startPoint.Y = i * (tileSize + wallSize) + wallSize + tileSize / 2;

                // While we are in the building - draw path.
                while (i < _bmap.Height && j < _bmap.Width && i >= 0 && j >= 0)
                {
                    endPointDraw.X = endPoint.X = startPoint.X;
                    endPointDraw.Y = endPoint.Y = startPoint.Y;
                    
                    switch (_fenotype[i * (int)_bmap.Width + j])
                    {
                        case Direction.UP:
                            endPoint.Y -= tileSize + wallSize;
                            endPointDraw.Y -= tileSize + (int)(wallSize * 1.5);
                            i--;
                            break;

                        case Direction.DOWN:
                            endPoint.Y += tileSize + wallSize;
                            endPointDraw.Y += tileSize + (int)(wallSize * 1.5);
                            i++;
                            break;

                        case Direction.LEFT:
                            endPoint.X -= tileSize + wallSize;
                            endPointDraw.X -= tileSize + (int)(wallSize * 1.5);
                            j--;
                            break;

                        case Direction.RIGHT:
                            endPoint.X += tileSize + wallSize;
                            endPointDraw.X += tileSize + (int)(wallSize * 1.5);
                            j++;
                            break;

                        default:
                            break;
                    }

                    g.DrawLine(pathPen, startPoint, endPointDraw);
                    startPoint.X = endPoint.X;
                    startPoint.Y = endPoint.Y;
                }
            }

            
            foreach (PeopleGroup group in _pmap.People)
            {
                int i = (int)group.Row;
                int j = (int)group.Col;

                float x = j * (tileSize + wallSize);
                float y = i * (tileSize + wallSize);

                PointF pf = new PointF(x, y);

                g.DrawString(group.Quantity.ToString(), peopleFont, peopleStringBrush, pf);
            }

            
            

            // Show image.
            uxImage.Width = b.Width;
            uxImage.Height = b.Height;
            uxImage.Image = b;
        }

        private void PreviewMap()
        {
            const int tileSize = 50;
            const int wallSize = 5;


            Bitmap b = new Bitmap((int)_bmap.Width * (tileSize + wallSize) + wallSize + 20,
                                  (int)_bmap.Height * (tileSize + wallSize) + wallSize + 20);

            Graphics g = Graphics.FromImage(b);

            // Background color
            g.Clear(Color.FromArgb(255, 230, 230, 230));

            // Pens
            Pen wallPen = new Pen(Color.FromArgb(255, 0, 0, 255), wallSize);
            Pen doorPen = new Pen(Color.Orange, wallSize);
            Pen pathPen = new Pen(Color.Green, wallSize);

            // Brushes
            SolidBrush tileBrush = new SolidBrush(Color.FromArgb(255, 200, 200, 200));
            SolidBrush doorStringBrush = new SolidBrush(Color.Black);

            // Fonts
            Font doorFont = new Font(FontFamily.GenericSansSerif, 10);

            // Draw tiles and walls/doors
            for (int i = 0; i < (int)_bmap.Height; i++)
            {
                for (int j = 0; j < (int)_bmap.Width; j++)
                {
                    if (_bmap.Floor[i][j] == null)
                        continue;
                    // Tile
                    g.FillRectangle(tileBrush,
                                    wallSize + j * (tileSize + wallSize),
                                    wallSize + i * (tileSize + wallSize),
                                    tileSize,
                                    tileSize);

                    // Doors/walls
                    foreach (Direction d in Enum.GetValues(typeof(Direction)))
                    {
                        // Nothing to draw.
                        if (!_bmap.Floor[i][j].GetSide(d).Draw)
                            continue;

                        // Prepare line coordinates.
                        Point p1 = new Point();
                        Point p2 = new Point();

                        p1.X = j * (tileSize + wallSize);
                        p1.Y = i * (tileSize + wallSize);

                        switch (d)
                        {
                            // Horizontal line (UP, DOWN)
                            case Direction.UP:
                                p1.Y += (int)Math.Floor(wallSize / 2.0);
                                p2.X = p1.X + tileSize + wallSize * 2;
                                p2.Y = p1.Y;
                                break;

                            case Direction.DOWN:
                                p1.Y += (int)Math.Floor(wallSize / 2.0);
                                p2.X = p1.X + tileSize + wallSize * 2;
                                p2.Y = p1.Y;
                                // Move line down
                                p1.Y += tileSize + wallSize;
                                p2.Y += tileSize + wallSize;
                                break;

                            // Vertical line (LEFT, RIGHT)
                            case Direction.LEFT:
                                p1.X += (int)Math.Floor(wallSize / 2.0);
                                p2.X = p1.X;
                                p2.Y = p1.Y + tileSize + wallSize * 2;
                                break;

                            case Direction.RIGHT:
                                p1.X += (int)Math.Floor(wallSize / 2.0);
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
                        {
                            // Draw doors
                            // Doors has to be shorter.
                            if (p1.Y == p2.Y)
                            {
                                // Horizontal line
                                p1.X += wallSize;
                                p2.X -= wallSize;
                            }
                            else
                            {
                                // Vertical line
                                p1.Y += wallSize;
                                p2.Y -= wallSize;
                            }

                            g.DrawLine(doorPen, p1, p2);
                            PointF pf = new PointF(p1.X, p1.Y);
                            g.DrawString(_bmap.Floor[i][j].GetSide(d).Capacity.ToString(), doorFont, doorStringBrush, pf);
                        }
                        else
                            // Draw wall
                            g.DrawLine(wallPen, p1, p2);
                    }
                }
            }

            // Show image.
            uxImage.Width = b.Width;
            uxImage.Height = b.Height;
            uxImage.Image = b;
        }

        private void uxRefreshButton_Click(object sender, EventArgs e)
        {
            XMLReader reader = new XMLReader();
            _bmap = reader.ReadBuildingMap("..\\..\\building_map2.abm");

            PreviewMap();
        }
    }
}

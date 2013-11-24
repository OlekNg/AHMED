using Common.DataModel.Enums;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BuildingEditor.Logic
{
    /// <summary>
    /// Represents single floor in the building.
    /// </summary>
    [Serializable]
    [ImplementPropertyChanged]
    public class Floor
    {
        public Floor()
        {
            Level = 0;
            Segments = new ObservableCollection<ObservableCollection<Segment>>();
        }

        /// <summary>
        /// Floor constructor.
        /// </summary>
        /// <param name="level">Floor level.</param>
        /// <param name="rows">Initial rows of floor.</param>
        /// <param name="cols">Initial cols of floor.</param>
        public Floor(int level, int rows = 5, int cols = 5)
            : this()
        {
            Level = level;

            for (int i = 0; i < rows; i++)
            {
                ObservableCollection<Segment> row = new ObservableCollection<Segment>();
                for (int j = 0; j < cols; j++)
                    row.Add(new Segment());

                Segments.Add(row);
            }

            ConnectSegments();
            RecalculateIndexes();
            UpdateRender();
        }

        public Floor(Common.DataModel.Floor floor)
            : this()
        {
            Level = floor.Level;

            for (int row = 0; row < floor.Segments.Count; row++)
            {
                ObservableCollection<Segment> segmentRow = new ObservableCollection<Segment>();
                for (int col = 0; col < floor.Segments[row].Count; col++)
                {
                    segmentRow.Add(new Segment(floor.Segments[row][col]));
                }
                Segments.Add(segmentRow);
            }

            ConnectSegments();
            RecalculateIndexes();
            UpdateRender();
        }        

        public ObservableCollection<ObservableCollection<Segment>> Segments { get; set; }
        public int Level { get; set; }

        /// <summary>
        /// Calculates number of floor type segments (available to genotype).
        /// </summary>
        public int GetFloorCount()
        {
            int result = 0;

            foreach (var row in Segments)
                foreach (var segment in row)
                    if (segment.Type == SegmentType.FLOOR) result++;

            return result;
        }

        /// <summary>
        /// Expands building in certain direction (adds row or column to all floors).
        /// </summary>
        /// <param name="side">Direction of expanding.</param>
        public void Expand(Direction side)
        {
            switch (side)
            {
                case Direction.RIGHT:
                    AddColumn(Segments[0].Count);
                    break;
                case Direction.DOWN:
                    AddRow(Segments.Count);
                    break;
                case Direction.LEFT:
                    AddColumn(0);
                    break;
                case Direction.UP:
                    AddRow(0);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Assigns to each segment proper Row and Column index. It is necessary for 
        /// most tools to work properly.
        /// Should be called after adding/deleting row/column.
        /// </summary>
        private void RecalculateIndexes()
        {
            for (int row = 0; row < Segments.Count; row++)
            {
                for (int col = 0; col < Segments[row].Count; col++)
                {
                    Segments[row][col].Row = row;
                    Segments[row][col].Column = col;
                }
            }
        }

        /// <summary>
        /// Updates outer walls and corners of each segments for correct
        /// rendering by wpf layout.
        /// </summary>
        public void UpdateRender()
        {
            for (int row = 0; row < Segments.Count; row++)
                for (int col = 0; col < Segments[row].Count; col++)
                    Segments[row][col].UpdateOuterWalls();

            for (int row = 0; row < Segments.Count; row++)
                for (int col = 0; col < Segments[row].Count; col++)
                    Segments[row][col].UpdateCorners();
        }

        /// <summary>
        /// Adds new row to floor at certain position.
        /// </summary>
        /// <param name="index">New row position.</param>
        public void AddRow(int index)
        {
            int cols = Segments[0].Count;
            ObservableCollection<Segment> row = new ObservableCollection<Segment>();
            for (int j = 0; j < cols; j++)
                row.Add(new Segment());

            Segments.Insert(index, row);
            ConnectSegments();
            RecalculateIndexes();
            UpdateRender();
        }

        /// <summary>
        /// Adds new column to floor at certain position.
        /// </summary>
        /// <param name="index">New column position.</param>
        public void AddColumn(int index)
        {
            int rows = Segments.Count;
            for (int i = 0; i < rows; i++)
                Segments[i].Insert(index, new Segment());

            ConnectSegments();
            RecalculateIndexes();
            UpdateRender();
        }

        public void RemoveRow(int index)
        {
            Segments.RemoveAt(index);
            ConnectSegments();
            RecalculateIndexes();
            UpdateRender();
        }

        public void RemoveColumn(int index)
        {
            foreach (ObservableCollection<Segment> row in Segments)
                row.RemoveAt(index);

            ConnectSegments();
            RecalculateIndexes();
            UpdateRender();
        }

        /// <summary>
        /// Connects all adjacent segments and their sides.
        /// Top side is down side of top segment.
        /// Left side is right side of left segment.
        /// </summary>
        private void ConnectSegments()
        {
            for (int row = 0; row < Segments.Count; row++)
            {
                for (int col = 0; col < Segments[row].Count; col++)
                {
                    var element = Segments[row][col];

                    element.LeftSegment = null;
                    element.TopSegment = null;
                    element.RightSegment = null;
                    element.BottomSegment = null;

                    if (row > 0)
                    {
                        // Connect side.
                        element.TopSide = Segments[row - 1][col].BottomSide;

                        // Connect adjacent segment.
                        element.TopSegment = Segments[row - 1][col];
                        Segments[row - 1][col].BottomSegment = element;
                    }

                    if (col > 0)
                    {
                        // Connect side.
                        Segments[row][col].LeftSide = Segments[row][col - 1].RightSide;

                        // Connect adjacent segment.
                        element.LeftSegment = Segments[row][col - 1];
                        Segments[row][col - 1].RightSegment = element;
                    }
                }
            }
        }

        public Common.DataModel.Floor ToDataModel()
        {
            Common.DataModel.Floor result = new Common.DataModel.Floor();

            result.Level = Level;

            for (int row = 0; row < Segments.Count; row++)
            {
                List<Common.DataModel.Segment> segmentRow = new List<Common.DataModel.Segment>();
                for (int col = 0; col < Segments[row].Count; col++)
                {
                    segmentRow.Add(Segments[row][col].ToDataModel());
                }
                result.Segments.Add(segmentRow);
            }

            return result;
        }
    }
}

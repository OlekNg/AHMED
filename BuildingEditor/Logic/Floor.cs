using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFTest.Logic
{
    /// <summary>
    /// Represents single floor in the building.
    /// </summary>
    [ImplementPropertyChanged]
    public class Floor
    {
        /// <summary>
        /// Floor constructor.
        /// </summary>
        /// <param name="level">Floor level.</param>
        /// <param name="rows">Initial rows of floor.</param>
        /// <param name="cols">Initial cols of floor.</param>
        public Floor(int level, int rows = 5, int cols = 5)
        {
            Level = level;
            Data = new ObservableCollection<ObservableCollection<Segment>>();

            for (int i = 0; i < rows; i++)
            {
                ObservableCollection<Segment> row = new ObservableCollection<Segment>();
                for (int j = 0; j < cols; j++)
                    row.Add(new Segment());

                Data.Add(row);
            }

            ConnectSegments();
            RecalculateIndexes();
            UpdateRender();
        }        

        public ObservableCollection<ObservableCollection<Segment>> Data { get; set; }
        public int Level { get; set; }

        /// <summary>
        /// Assigns to each segment proper Row and Column index. It is necessary for 
        /// most tools to work properly.
        /// Should be called after adding/deleting row/column.
        /// </summary>
        private void RecalculateIndexes()
        {
            for (int row = 0; row < Data.Count; row++)
            {
                for (int col = 0; col < Data[row].Count; col++)
                {
                    Data[row][col].Row = row;
                    Data[row][col].Column = col;
                }
            }
        }

        /// <summary>
        /// Updates outer walls and corners of each segments for correct
        /// rendering by wpf layout.
        /// </summary>
        public void UpdateRender()
        {
            for (int row = 0; row < Data.Count; row++)
                for (int col = 0; col < Data[row].Count; col++)
                    Data[row][col].UpdateOuterWalls();

            for (int row = 0; row < Data.Count; row++)
                for (int col = 0; col < Data[row].Count; col++)
                    Data[row][col].UpdateCorners();
        }

        /// <summary>
        /// Adds new row to floor at certain position.
        /// </summary>
        /// <param name="index">New row position.</param>
        public void AddRow(int index)
        {
            int cols = Data[0].Count;
            ObservableCollection<Segment> row = new ObservableCollection<Segment>();
            for (int j = 0; j < cols; j++)
                row.Add(new Segment());

            Data.Insert(index, row);
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
            int rows = Data.Count;
            for (int i = 0; i < rows; i++)
                Data[i].Insert(index, new Segment());

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
            for (int row = 0; row < Data.Count; row++)
            {
                for (int col = 0; col < Data[row].Count; col++)
                {
                    var element = Data[row][col];

                    if (row > 0)
                    {
                        // Connect side.
                        element.TopSide = Data[row - 1][col].BottomSide;

                        // Connect adjacent segment.
                        element.TopSegment = Data[row - 1][col];
                        Data[row - 1][col].BottomSegment = element;
                    }

                    if (col > 0)
                    {
                        // Connect side.
                        Data[row][col].LeftSide = Data[row][col - 1].RightSide;

                        // Connect adjacent segment.
                        element.LeftSegment = Data[row][col - 1];
                        Data[row][col - 1].RightSegment = element;
                    }
                }
            }
        }
    }
}

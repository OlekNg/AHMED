using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFTest.Logic
{
    [ImplementPropertyChanged]
    public class Floor
    {
        public Floor(int number, int rows = 5, int cols = 5)
        {
            Number = number;
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
        public int Number { get; set; }

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

                        // Connect neighbours.
                        element.TopSegment = Data[row - 1][col];
                        Data[row - 1][col].BottomSegment = element;
                    }

                    if (col > 0)
                    {
                        // Connect side.
                        Data[row][col].LeftSide = Data[row][col - 1].RightSide;

                        // Connect neighbours.
                        element.LeftSegment = Data[row][col - 1];
                        Data[row][col - 1].RightSegment = element;
                    }
                }
            }
        }

        public void UpdateRender()
        {
            for (int row = 0; row < Data.Count; row++)
                for (int col = 0; col < Data[row].Count; col++)
                    Data[row][col].UpdateOuterWalls();

            for (int row = 0; row < Data.Count; row++)
                for (int col = 0; col < Data[row].Count; col++)
                    Data[row][col].UpdateCorners();
        }

        public void AddRow(int index)
        {
            int cols = Data[0].Count;
            ObservableCollection<Segment> row = new ObservableCollection<Segment>();
            for (int j = 0; j < cols; j++)
                row.Add(new Segment());

            Data.Insert(index, row);
            ConnectSegments();
            RecalculateIndexes();
        }

        public void AddColumn(int index)
        {
            int rows = Data.Count;
            for (int i = 0; i < rows; i++)
                Data[i].Insert(index, new Segment());

            ConnectSegments();
            RecalculateIndexes();
        }
    }
}

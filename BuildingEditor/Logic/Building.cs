using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFTest.Logic
{
    public class Building
    {
        public Building(int width = 5, int height = 5)
        {
            Data = new ObservableCollection<ObservableCollection<Segment>>();

            for (int i = 0; i < height; i++)
            {
                ObservableCollection<Segment> row = new ObservableCollection<Segment>();
                for (int j = 0; j < width; j++)
                    row.Add(new Segment() { Row = i, Column = j });

                Data.Add(row);
            }

            ConnectSegments();
            Data[4][4].Type = SegmentType.NONE;
            UpdateBuilding();
        }

        public ObservableCollection<ObservableCollection<Segment>> Data { get; set; }

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

        public void UpdateBuilding()
        {
            for (int row = 0; row < Data.Count; row++)
                for (int col = 0; col < Data[row].Count; col++)
                    Data[row][col].UpdateOuterWalls();

            for (int row = 0; row < Data.Count; row++)
                for (int col = 0; col < Data[row].Count; col++)
                    Data[row][col].UpdateCorners();
        }
    }
}

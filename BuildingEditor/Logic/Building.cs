using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFTest.Logic
{
    [ImplementPropertyChanged]
    public class Building
    {
        private int _cols;
        private int _rows;

        public Building(int rows = 5, int cols = 5)
        {
            _rows = rows;
            _cols = cols;

            Floors = new ObservableCollection<Floor>();
            Floors.Add(new Floor(0, _rows, _cols));
        }

        public ObservableCollection<Floor> Floors { get; set; }

        public Floor CurrentFloor { get; set; }

        public void Expand(Side side)
        {
            switch (side)
            {
                case Side.RIGHT:
                    AddColumn(_cols);
                    break;
                case Side.BOTTOM:
                    AddRow(_rows);
                    break;
                case Side.LEFT:
                    AddColumn(0);
                    break;
                case Side.TOP:
                    AddRow(0);
                    break;
                default:
                    break;
            }
        }

        private void AddRow(int index)
        {
            foreach (var floor in Floors)
                floor.AddRow(index);
        }

        private void AddColumn(int index)
        {
            foreach (var floor in Floors)
                floor.AddColumn(index);
        }
    }
}

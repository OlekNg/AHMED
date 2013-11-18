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

        /// <summary>
        /// Creates building with one floor.
        /// </summary>
        /// <param name="rows">Initial rows of building.</param>
        /// <param name="cols">Initial columns of building.</param>
        public Building(int rows = 5, int cols = 5)
        {
            _rows = rows;
            _cols = cols;

            Floors = new ObservableCollection<Floor>();
            AddFloor();
        }

        /// <summary>
        /// Floors in the building.
        /// </summary>
        public ObservableCollection<Floor> Floors { get; set; }

        /// <summary>
        /// Currently selected floor in editor.
        /// </summary>
        public Floor CurrentFloor { get; set; }

        /// <summary>
        /// Adds new floor to building.
        /// </summary>
        public void AddFloor()
        {
            Floors.Insert(0, new Floor(Floors.Count, _rows, _cols));
        }

        public void Expand(Side side)
        {
            foreach (Floor f in Floors)
            {
                f.Expand(side);
            }

            _rows = Floors[0].Data.Count;
            _cols = Floors[0].Data[0].Count;
        }

        /// <summary>
        /// Adds new row to building at certain position.
        /// Applies to all floors.
        /// </summary>
        /// <param name="index">New row position.</param>
        private void AddRow(int index)
        {
            foreach (var floor in Floors)
                floor.AddRow(index);
        }

        /// <summary>
        /// Adds new column to building at ceratin position.
        /// Applies to all floors.
        /// </summary>
        /// <param name="index">New column position.</param>
        private void AddColumn(int index)
        {
            foreach (var floor in Floors)
                floor.AddColumn(index);
        }

        public void RemoveRow(int index)
        {
            foreach (var floor in Floors)
                floor.RemoveRow(index);
        }

        public void RemoveColumn(int index)
        {
            foreach (var floor in Floors)
                floor.RemoveRow(index);
        }
    }
}

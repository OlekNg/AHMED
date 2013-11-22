using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BuildingEditor.Logic
{
    [Serializable]
    [ImplementPropertyChanged]
    public class Building
    {
        public Building()
        {
            Floors = new ObservableCollection<Floor>();
            Stairs = new ObservableCollection<StairsPair>();
        }

        /// <summary>
        /// Creates building with one floor.
        /// </summary>
        /// <param name="rows">Initial rows of building.</param>
        /// <param name="cols">Initial columns of building.</param>
        public Building(int rows = 5, int cols = 5)
            : this()
        {
            AddFloor(rows, cols);
        }

        public Building(DataModel.Building building)
            : this()
        {
            for (int i = 0; i < building.Floors.Count; i++)
                Floors.Add(new Floor(building.Floors[i]));

            for (int i = 0; i < building.Stairs.Count; i++)
            {
                var temp = building.Stairs[i];
                var stairsPair = new StairsPair(Stairs, temp);
                stairsPair.First.AssignedSegment = Floors.Where(x => x.Level == temp.First.Level).First().Data[temp.First.Row][temp.First.Col];
                stairsPair.Second.AssignedSegment = Floors.Where(x => x.Level == temp.Second.Level).First().Data[temp.Second.Row][temp.Second.Col];
                stairsPair.SetAdditionalData();
                Stairs.Add(stairsPair);
            }


        }

        /// <summary>
        /// Floors in the building.
        /// </summary>
        public ObservableCollection<Floor> Floors { get; set; }

        public ObservableCollection<StairsPair> Stairs { get; set; }

        /// <summary>
        /// Currently selected floor in editor.
        /// </summary>
        public Floor CurrentFloor { get; set; }

        /// <summary>
        /// Adds new floor to building.
        /// </summary>
        public void AddFloor(int rows, int cols)
        {
            Floors.Insert(0, new Floor(Floors.Count, rows, cols));
        }
    }
}

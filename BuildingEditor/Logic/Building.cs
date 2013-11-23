using Common.DataModel.Enums;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BuildingEditor.Logic
{
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

        public Building(Common.DataModel.Building building)
            : this()
        {
            for (int i = 0; i < building.Floors.Count; i++)
                Floors.Add(new Floor(building.Floors[i]));

            for (int i = 0; i < building.Stairs.Count; i++)
            {
                var temp = building.Stairs[i];
                var stairsPair = new StairsPair(Stairs, temp);
                stairsPair.First.AssignedSegment = Floors.Where(x => x.Level == temp.First.Level).First().Segments[temp.First.Row][temp.First.Col];
                stairsPair.Second.AssignedSegment = Floors.Where(x => x.Level == temp.Second.Level).First().Segments[temp.Second.Row][temp.Second.Col];
                stairsPair.SetAdditionalData();
                Stairs.Add(stairsPair);
            }
        }

        public int GetFloorCount()
        {
            int result = 0;

            foreach (var f in Floors)
                result += f.GetFloorCount();

            return result;
        }

        public void SetFenotype(List<Side> fenotype)
        {
            int expectedLength = GetFloorCount();

            if (fenotype.Count != expectedLength)
                throw new Exception("Invalid fenotype to set.");

            List<Floor> floors = Floors.Reverse().ToList();

            int index = 0;
            foreach(var f in floors)
                foreach(var row in f.Segments)
                    foreach(var segment in row)
                        if (segment.Type == SegmentType.FLOOR)
                        {
                            segment.Fenotype = fenotype[index];
                            index++;
                        }
        }

        public Common.DataModel.Building ToDataModel()
        {
            Common.DataModel.Building result = new Common.DataModel.Building();

            for (int i = 0; i < Floors.Count; i++)
                result.Floors.Add(Floors[i].ToDataModel());

            for (int i = 0; i < Stairs.Count; i++)
                result.Stairs.Add(Stairs[i].ToDataModel());

            return result;
        }

        /// <summary>
        /// Floors in the building.
        /// </summary>
        public ObservableCollection<Floor> Floors { get; set; }

        public ObservableCollection<StairsPair> Stairs { get; set; }

        public string ViewMode { get; set; }

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

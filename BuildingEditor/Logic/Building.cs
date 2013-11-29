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
    /// Represents view model of building that can be used in wpf gui applications.
    /// </summary>
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

        /// <summary>
        /// Creates building from data model.
        /// </summary>
        /// <param name="building">Data model building.</param>
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

        /// <summary>
        /// Sets Solution to true only in segments which are used by people to escape.
        /// </summary>
        public void DrawSolution()
        {
            // Clear old solution.
            foreach (var floor in Floors)
                foreach (var row in floor.Segments)
                    foreach (var segment in row)
                        segment.Solution = false;

            foreach (var floor in Floors)
                foreach (var row in floor.Segments)
                    foreach (var segment in row)
                        if (segment.Type == SegmentType.FLOOR && segment.PeopleCount > 0)
                            DrawSolutionPath(segment);
        }

        protected void DrawSolutionPath(Segment segment)
        {
            while (segment != null && segment.Solution == false)
            {
                // If encountered stairs, then move to segment where are second stairs from pair.
                if (segment.Type == SegmentType.STAIRS)
                {
                    StairsPair pair = segment.AdditionalData as StairsPair;
                    if (segment == pair.First.AssignedSegment)
                        segment = pair.Second.AssignedSegment;
                    else
                        segment = pair.First.AssignedSegment;

                    // Stairs have no fenotype, so we have to use orientation.
                    segment.Solution = true;
                    segment = segment.GetNeighbour(segment.Orientation);
                }

                segment.Solution = true;
                segment = segment.GetNeighbour(segment.Fenotype);
            }
        }

        /// <summary>
        /// Counts all segments of type floor.
        /// </summary>
        /// <returns>Number of floor type segments.</returns>
        public int GetFloorCount()
        {
            int result = 0;

            foreach (var f in Floors)
                result += f.GetFloorCount();

            return result;
        }

        /// <summary>
        /// Calculates normal fenotype of building (only floor-type segments).
        /// </summary>
        /// <returns>Fenotype.</returns>
        public List<Direction> GetFenotype()
        {
            List<Direction> result = new List<Direction>();

            // Order floors by level to make sure we are creating fenotype
            // from bottom to top level of building.
            List<Floor> floors = Floors.OrderBy(x => x.Level).ToList();

            // Add fenotype from all segments of type floor.
            foreach (var floor in floors)
            {
                foreach (var row in floor.Segments)
                    foreach (var segment in row)
                        if(segment.Type == SegmentType.FLOOR)
                            result.Add(segment.Fenotype);
            }

            return result;
        }

        public List<Segment> GetPeopleGroups()
        {
            List<Segment> result = new List<Segment>();

            foreach (var floor in Floors)
                result.AddRange(floor.GetPeopleGroups());

            return result;
        }

        /// <summary>
        /// Creates fenotype for simulator which requires all segments (even of type NONE)
        /// to be covered by fenotype.
        /// 
        /// If you didn't set normal fenotype (for floor segments only) for building use function variant with parameter.
        /// </summary>
        /// <returns>Fenotype for simulator.</returns>
        public List<List<Direction>> GetSimulatorFenotype()
        {
            List<List<Direction>> result = new List<List<Direction>>();

            // Order floors by level to make sure we are creating fenotype
            // from bottom to top level of building.
            List<Floor> floors = Floors.OrderBy(x => x.Level).ToList();

            // Add fenotype from all segments (type of segment doesn't matter).
            foreach (var floor in floors)
            {
                List<Direction> floorFenotype = new List<Direction>();

                foreach (var row in floor.Segments)
                    foreach (var segment in row)
                        floorFenotype.Add(segment.Fenotype);

                result.Add(floorFenotype);
            }

            return result;
        }

        /// <summary>
        /// Sets fenotype for building view model. And calls GetSimulatorFenotype().
        /// </summary>
        /// <param name="fenotype">Fenotype that covers only segments of type floor.</param>
        /// <returns>Fenotype for simulator.</returns>
        public List<List<Direction>> GetSimulatorFenotype(List<Direction> fenotype)
        {
            SetFenotype(fenotype);
            return GetSimulatorFenotype();
        }

        /// <summary>
        /// Sets fenotype for this building.
        /// </summary>
        /// <param name="fenotype">Fenotype that covers only floor type segments.</param>
        /// <see cref="GetFloorCount"/>
        public void SetFenotype(List<Direction> fenotype)
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

        /// <summary>
        /// Converts building view model to data model that can be stored in xml file.
        /// </summary>
        /// <see cref="Common.DataModel.Building"/>
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

        /// <summary>
        /// Paired stairs in the building.
        /// </summary>
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

        /// <summary>
        /// Removes floor from building.
        /// </summary>
        /// <param name="level">Level to remove.</param>
        internal void RemoveFloor(int level)
        {
            Floor f = Floors.Where(x => x.Level == level).First();
            Floors.Remove(f);

            // Update higher floors level number.
            Floors.Where(x => x.Level > f.Level).ToList().ForEach(x => x.Level--);

            // Delete all stairs that were in floor that has been deleted.
            var stairsToDelete = Stairs.Where(x => x.First.Level == f.Level || x.Second.Level == f.Level).ToList();
            stairsToDelete.ForEach(x => x.Destroy());

            // Update stairs levels.
            Stairs.ToList().ForEach(x =>
            {
                if (x.First.Level > level)
                    x.First.Level--;

                if (x.Second.Level > level)
                    x.Second.Level--;
            });
        }

        public int GetPeopleCount()
        {
            int result = 0;

            foreach (var f in Floors)
                result += f.GetPeopleCount();

            return result;
        }
    }
}

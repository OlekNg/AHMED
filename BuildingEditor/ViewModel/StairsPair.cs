﻿using Common.DataModel.Enums;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BuildingEditor.ViewModel
{
    [ImplementPropertyChanged]
    public class Stairs
    {
        public Stairs()
        {
            Level = 0;
            Capacity = 3;
            Delay = 1;
            EntranceCapacity = 3;
        }

        public Stairs(Common.DataModel.Stairs stairs)
        {
            Level = stairs.Level;
            Capacity = stairs.Capacity;
            EntranceCapacity = stairs.EntranceCapacity;
            Delay = stairs.Delay;
        }

        public Stairs(Stairs other)
        {
            Level = other.Level;
            Capacity = other.Capacity;
            EntranceCapacity = other.EntranceCapacity;
            Delay = other.Delay;
        }

        public int Level { get; set; }
        public int EntranceCapacity { get; set; }
        public int Capacity { get; set; }
        public int Delay { get; set; }

        public Segment AssignedSegment { get; set; }

        public Common.DataModel.Stairs ToDataModel()
        {
            Common.DataModel.Stairs result = new Common.DataModel.Stairs();

            result.EntranceCapacity = EntranceCapacity;
            result.Capacity = Capacity;
            result.Delay = Delay;
            result.Level = Level;
            result.Row = AssignedSegment.Row;
            result.Col = AssignedSegment.Column;
            result.Orientation = AssignedSegment.Orientation;

            return result;
        }
    }

    [ImplementPropertyChanged]
    public class StairsPair
    {
        private ObservableCollection<StairsPair> _stairs;

        public StairsPair()
        {
            _stairs = new ObservableCollection<StairsPair>();
        }

        public StairsPair(ObservableCollection<StairsPair> stairs)
        {
            _stairs = stairs;
        }

        public StairsPair(ObservableCollection<StairsPair> stairs, Common.DataModel.StairsPair stairsPair)
        {
            _stairs = stairs;
            First = new Stairs(stairsPair.First);
            Second = new Stairs(stairsPair.Second);
        }

        public StairsPair(ObservableCollection<StairsPair> stairs, StairsPair other)
        {
            _stairs = stairs;
            First = new Stairs(other.First);
            Second = new Stairs(other.Second);
        }

        public Stairs First { get; set; }
        public Stairs Second { get; set; }

        public void SetAdditionalData()
        {
            First.AssignedSegment.AdditionalData = this;
            Second.AssignedSegment.AdditionalData = this;
        }

        public void Destroy()
        {
            First.AssignedSegment.Type = SegmentType.FLOOR;
            Second.AssignedSegment.Type = SegmentType.FLOOR;
            _stairs.Remove(this);
        }

        internal Common.DataModel.StairsPair ToDataModel()
        {
            Common.DataModel.StairsPair result = new Common.DataModel.StairsPair();

            result.First = First.ToDataModel();
            result.Second = Second.ToDataModel();

            return result;
        }
    }
}

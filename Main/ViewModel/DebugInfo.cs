using BuildingEditor.ViewModel;
using Genetics.Evaluators;
using PropertyChanged;
using Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genetics;

namespace Main.ViewModel
{
    [ImplementPropertyChanged]
    public class DebugInfo
    {
        EvaCalcEvaluator _evaluator;
        Building _building;

        public DebugInfo()
        {
        }

        protected void BuildEvaluator()
        {
            MapBuilder mapBuilder = new MapBuilder(_building.ToDataModel());
            Simulator sim = new Simulator();
            sim.MaximumTicks = _building.GetFloorCount() * 2;
            sim.SetupSimulator(mapBuilder.BuildBuildingMap(), mapBuilder.BuildPeopleMap());
            _evaluator = new EvaCalcEvaluator(sim, _building, true);
        }

        public Building Building
        {
            get { return _building; }
            set { _building = value; BuildEvaluator(); }
        }
        public double CurrentFenotypeValue { get; set; }

        public void Update()
        {
            if (_evaluator != null)
                CurrentFenotypeValue = _evaluator.Eval(Building.GetFenotype().ToGenotype());
        }
    }
}

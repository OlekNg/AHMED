using BuildingEditor.ViewModel;
using Genetics;
using Genetics.Evaluators;
using Genetics.Operators;
using Genetics.Specialized;
using PropertyChanged;
using Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Main.ViewModel.GeneticsConfiguration
{
    [ImplementPropertyChanged]
    public class LocalOptimizationConfiguration : IGUIConfiguration, ITransformationConfiguration
    {
        public LocalOptimizationConfiguration()
        {
        }

        public string Name { get { return "Local optimization"; } }

        public System.Windows.FrameworkElement GUI { get; set; }
    
        #region ITransformationConfiguration Members
        public Genetics.Generic.ITransformer<List<bool>> BuildTransformer(BuildingEditor.ViewModel.Building building)
        {
            MapBuilder mapBuilder = new MapBuilder(building.ToDataModel());
            Simulator sim = new Simulator();

            sim.MaximumTicks = building.GetFloorCount() * 2;
            sim.SetupSimulator(mapBuilder.BuildBuildingMap(), mapBuilder.BuildPeopleMap());
            EvaCalcEvaluator evaluator = new EvaCalcEvaluator(sim, new Building(building));

            return new LocalOptimization(building, evaluator);
        }
        #endregion
    }
}

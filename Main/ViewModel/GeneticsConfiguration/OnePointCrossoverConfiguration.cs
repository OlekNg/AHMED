using Genetics.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Main.ViewModel.GeneticsConfiguration
{
    public class OnePointCrossoverConfiguration : IGUIConfiguration, ICrossoverConfiguration
    {
        public Genetics.Generic.ICrossoverOperator<List<bool>> BuildCrossoverOperator(BuildingEditor.ViewModel.Building building)
        {
            return new OnePointCrossover();
        }

        public System.Windows.FrameworkElement BuildGUIConfiguration()
        {
            return null;
        }

        public string Name { get { return "One point"; } }


        public System.Windows.FrameworkElement GUI { get; set; }
    }
}

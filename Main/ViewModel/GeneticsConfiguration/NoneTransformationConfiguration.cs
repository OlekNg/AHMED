using Genetics.Operators;
using Genetics.Specialized;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Main.ViewModel.GeneticsConfiguration
{
    [ImplementPropertyChanged]
    public class NoneTransformationConfiguration : IGUIConfiguration, ITransformationConfiguration
    {
        public NoneTransformationConfiguration()
        {
            GUI = BuildGUIConfiguration();
        }

        public System.Windows.FrameworkElement BuildGUIConfiguration()
        {
            return null;
        }

        public string Name { get { return "None"; } }

        public System.Windows.FrameworkElement GUI { get; set; }

        #region ITransformationConfiguration Members

        public Genetics.Generic.ITransformer<List<bool>> BuildTransformer(BuildingEditor.ViewModel.Building building)
        {
            return null;
        }

        #endregion
    }
}

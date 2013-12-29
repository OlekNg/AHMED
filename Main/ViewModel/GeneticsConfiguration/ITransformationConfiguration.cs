using BuildingEditor.ViewModel;
using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Main.ViewModel.GeneticsConfiguration
{
    public interface ITransformationConfiguration
    {
        ITransformer<List<bool>> BuildTransformer(Building building);
    }
}

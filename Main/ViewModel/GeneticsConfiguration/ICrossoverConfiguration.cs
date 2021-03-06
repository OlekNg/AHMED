﻿using Genetics.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Main.ViewModel.GeneticsConfiguration
{
    public interface ICrossoverConfiguration
    {
        ICrossoverOperator<List<bool>> BuildCrossoverOperator(BuildingEditor.ViewModel.Building building);
    }
}

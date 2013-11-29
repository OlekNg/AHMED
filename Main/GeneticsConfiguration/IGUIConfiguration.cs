using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Main.GeneticsConfiguration
{
    public interface IGUIConfiguration
    {
        string Name { get; }
        FrameworkElement GUI { get; set; }
    }
}

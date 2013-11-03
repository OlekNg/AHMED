using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPFTest.Logic
{
    public class ToolSegmentArg
    {
        public int SegmentRow { get; set; }
        public int SegmentCol { get; set; }
        public Point MousePosition { get; set; }
        public Point SegmentRenderedSize { get; set; }
    }

    [ImplementPropertyChanged]
    public abstract class Tool
    {
        public string Name { get; set; }

        public abstract void MouseDown(object sender, MouseButtonEventArgs e);
        public abstract void MouseMove(object sender, MouseButtonEventArgs e);
        public abstract void MouseUp(object sender, MouseButtonEventArgs e);
    }
}

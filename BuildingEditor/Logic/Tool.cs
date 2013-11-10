using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

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
        public bool Clear { get; set; }

        public abstract void CancelAction();
        public abstract void MouseDown(object sender, MouseButtonEventArgs e);
        public abstract void MouseMove(object sender, MouseEventArgs e);
        public abstract void MouseUp(object sender, MouseButtonEventArgs e);
        public abstract void MouseEnter(object sender, MouseEventArgs e);
        public abstract void MouseLeave(object sender, MouseEventArgs e);

        protected Segment SenderToSegment(object sender)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null)
                return null;

            Segment segment = element.Tag as Segment;
            return segment;
        }
    }
}

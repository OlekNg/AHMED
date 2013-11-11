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
        public Tool()
        {
            Name = "Tool";
            Configuration = BuildConfiguration();
        }

        public string Name { get; set; }

        /// <summary>
        /// Configuration that will appear in tool details section below toolbox.
        /// To implement this for inherited tool override BuildConfiguration method.
        /// </summary>
        public FrameworkElement Configuration { get; set; }

        public virtual void CancelAction() { }
        public virtual void MouseDown(object sender, MouseButtonEventArgs e) { }
        public virtual void MouseMove(object sender, MouseEventArgs e) { }
        public virtual void MouseUp(object sender, MouseButtonEventArgs e) { }
        public virtual void MouseEnter(object sender, MouseEventArgs e) { }
        public virtual void MouseLeave(object sender, MouseEventArgs e) { }

        protected Segment SenderToSegment(object sender)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null)
                return null;

            Segment segment = element.Tag as Segment;
            return segment;
        }

        /// <summary>
        /// Through this method you can expose an interface for additional
        /// configuration of your tool.
        /// </summary>
        protected virtual FrameworkElement BuildConfiguration()
        {
            return null;
        }
    }
}

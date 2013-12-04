using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace BuildingEditor.ViewModel
{
    public interface ISegmentEventHandler
    {
        void Segment_MouseDown(object sender, MouseButtonEventArgs e);
        void Segment_MouseMove(object sender, MouseEventArgs e);
        void Segment_MouseUp(object sender, MouseButtonEventArgs e);
        void Segment_MouseEnter(object sender, MouseEventArgs e);
        void Segment_MouseLeave(object sender, MouseEventArgs e);
        void Segment_MouseWheel(object sender, MouseWheelEventArgs e);
    }

    public partial class SegmentEventHandler
    {
        private static List<ISegmentEventHandler> _handlers = new List<ISegmentEventHandler>();

        public SegmentEventHandler()
        {
        }

        public static void Register(ISegmentEventHandler handler)
        {
            _handlers.Add(handler);
        }

        private void Segment_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (ISegmentEventHandler h in _handlers)
                h.Segment_MouseDown(sender, e);
        }

        private void Segment_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (ISegmentEventHandler h in _handlers)
                h.Segment_MouseMove(sender, e);
        }

        private void Segment_MouseUp(object sender, MouseButtonEventArgs e)
        {
            foreach (ISegmentEventHandler h in _handlers)
                h.Segment_MouseUp(sender, e);
        }

        private void Segment_MouseEnter(object sender, MouseEventArgs e)
        {
            foreach (ISegmentEventHandler h in _handlers)
                h.Segment_MouseEnter(sender, e);
        }

        private void Segment_MouseLeave(object sender, MouseEventArgs e)
        {
            foreach (ISegmentEventHandler h in _handlers)
                h.Segment_MouseLeave(sender, e);
        }

        private void Segment_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            foreach (ISegmentEventHandler h in _handlers)
                h.Segment_MouseWheel(sender, e);
        }
    }
}

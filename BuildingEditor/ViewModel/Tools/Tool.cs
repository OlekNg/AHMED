﻿using BuildingEditor.ViewModel;
using Common.DataModel.Enums;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace BuildingEditor.ViewModel.Tools
{
    [ImplementPropertyChanged]
    public abstract class Tool
    {
        public Tool()
        {
            Name = "Tool";
            GUIConfiguration = BuildGUIConfiguration();
        }

        /// <summary>
        /// Tool name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Configuration that will appear in tool details section below toolbox.
        /// To implement this for inherited tool override BuildConfiguration method.
        /// </summary>
        public FrameworkElement GUIConfiguration { get; set; }

        public virtual void CancelAction() { }
        public virtual void ClearPreview() { }
        public virtual void MouseDown(object sender, MouseButtonEventArgs e) { }
        public virtual void MouseMove(object sender, MouseEventArgs e) { }
        public virtual void MouseUp(object sender, MouseButtonEventArgs e) { }
        public virtual void MouseEnter(object sender, MouseEventArgs e) { }
        public virtual void MouseLeave(object sender, MouseEventArgs e) { }
        public virtual void MouseWheel(object sender, MouseWheelEventArgs e) { }

        protected Segment SenderToSegment(object sender)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null)
                return null;

            Segment segment = element.Tag as Segment;
            return segment;
        }

        /// <summary>
        /// Caluclates which side (triangle) of segment has been clicked.
        /// </summary>
        /// <param name="size">Size of segment (rendered width/height - assumption that segment is square).</param>
        /// <param name="pos">Mouse click location relative to segment.</param>
        /// <returns></returns>
        protected Direction CalculateSegmentSide(double size, Point pos)
        {
            if (pos.X > pos.Y && pos.X < (size - pos.Y))
                return Direction.UP;

            if (pos.X > pos.Y && pos.X > (size - pos.Y))
                return Direction.RIGHT;

            if (pos.X < pos.Y && pos.X < (size - pos.Y))
                return Direction.LEFT;

            if (pos.X < pos.Y && pos.X > (size - pos.Y))
                return Direction.DOWN;

            return Direction.DOWN;
        }

        protected SegmentSide ProcessEventArg(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return null;

            Segment segment = element.Tag as Segment;
            if (segment == null) return null;

            Point pos = e.GetPosition(element);
            var size = element.ActualHeight;

            Direction side = CalculateSegmentSide(size, pos);

            pos.X -= (size / 2);
            pos.Y -= (size / 2);

            return new SegmentSide { Segment = segment, Side = side, MousePosition = pos };
        }

        /// <summary>
        /// Through this method you can expose an interface for additional
        /// configuration of your tool.
        /// </summary>
        protected virtual FrameworkElement BuildGUIConfiguration()
        {
            return null;
        }

        
    }
}

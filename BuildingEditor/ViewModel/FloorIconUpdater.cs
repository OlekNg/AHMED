using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BuildingEditor.ViewModel
{
    /// <summary>
    /// Updates floor icons - currently not used because of troubles with
    /// desired multithreading.
    /// </summary>
    public class FloorIconUpdater
    {
        private Building _building;
        private Floor _lastSelectedFloor;
        private BackgroundWorker _bw;

        public FloorIconUpdater(Building building, ListView floors)
        {
            _building = building;
            _lastSelectedFloor = _building.CurrentFloor;
            _bw = new BackgroundWorker();
            _bw.DoWork += Work;
            floors.SelectionChanged += new SelectionChangedEventHandler(OnFloorChanged);
            UpdateAll();
        }

        private void Work(object sender, DoWorkEventArgs e)
        {
            var arg = e.Argument as SelectionChangedEventArgs;
            UpdateLastSelected();
            _lastSelectedFloor = arg.AddedItems.Count > 0 ? arg.AddedItems[0] as Floor : null;
        }

        private void OnFloorChanged(object sender, SelectionChangedEventArgs e)
        {
            Thread t = new Thread(ThreadProc);
            t.SetApartmentState(ApartmentState.STA);

            t.Start(e);
        }

        protected void UpdateAll()
        {
            foreach (var f in _building.Floors)
                CreateIcon(f);
        }

        protected void UpdateLastSelected()
        {
            Thread t = new Thread(ThreadProc);
            t.SetApartmentState(ApartmentState.STA);

            t.Start();

            if (_lastSelectedFloor != null)
                CreateIcon(_lastSelectedFloor);
        }

        private void ThreadProc(object obj)
        {
            var arg = obj as SelectionChangedEventArgs;
            UpdateLastSelected();
            _lastSelectedFloor = arg.AddedItems.Count > 0 ? arg.AddedItems[0] as Floor : null;
        }

        protected void CreateIcon(Floor floor)
        {
            ItemsControl control = new ItemsControl();
            Canvas canvas = new Canvas();
            control.ItemTemplate = Application.Current.FindResource("SegmentRowTemplate") as DataTemplate;
            canvas.Children.Add(control);

            control.ItemsSource = floor.Segments;
            control.UpdateLayout();
            canvas.Measure(new Size(1000, 1000));
            canvas.Arrange(new Rect(new Size(1000, 1000)));

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)control.DesiredSize.Width, (int)control.DesiredSize.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(control);

            floor.Icon = bmp;
        }
    }
}

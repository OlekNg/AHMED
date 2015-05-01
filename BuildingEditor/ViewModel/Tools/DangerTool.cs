using BuildingEditor.ViewModel;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace BuildingEditor.ViewModel.Tools
{
    [ImplementPropertyChanged]
    public class DangerTool : Tool
    {
        private Editor _editor;

        public DangerTool(Editor editor)
        {
            Name = "Danger";
            _editor = editor;
        }

        public override void MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var segment = SenderToSegment(sender);
            segment.Danger = !segment.Danger;
            _editor.CurrentBuilding.CurrentFloor.UpdateDangerLevels();
        }
    }
}

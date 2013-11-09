using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFTest.Logic
{
    public enum SideElementType { NONE, WALL, DOOR }

    [ImplementPropertyChanged]
    public class SideElement
    {
        public SideElement(SideElementType type = SideElementType.NONE)
        {
            Type = type;
        }

        #region Properties
        public int Capacity { get; set; }
        public SideElementType Type { get; set; }
        public bool PreviewWall { get; set; }
        public bool PreviewDoor { get; set; }
        #endregion
    }
}

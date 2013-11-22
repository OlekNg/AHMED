using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BuildingEditor.Logic
{
    public enum SideElementType { NONE, WALL, DOOR }

    [ImplementPropertyChanged]
    public class SideElement
    {
        public SideElement()
        {
            Type = SideElementType.NONE;
        }

        public SideElement(SideElementType type = SideElementType.NONE)
        {
            Type = type;
        }

        public SideElement(DataModel.SideElement sideElement)
        {
            Type = sideElement.Type;
            Capacity = sideElement.Capacity;
        }

        #region Properties
        public int Capacity { get; set; }
        public SideElementType Type { get; set; }

        public SideElementType PreviewType { get; set; }
        public bool Preview { get; set; }
        #endregion
    }
}

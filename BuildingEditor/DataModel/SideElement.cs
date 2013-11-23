using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingEditor.DataModel
{
    [Serializable]
    public class SideElement
    {
        public BuildingEditor.Logic.SideElementType Type;
        public int Capacity;

        public SideElement() { }

        public SideElement(Logic.SideElement sideElement)
        {
            Type = sideElement.Type;
            Capacity = sideElement.Capacity;
        }
    }
}

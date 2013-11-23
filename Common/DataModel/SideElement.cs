using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DataModel
{
    [Serializable]
    public class SideElement
    {
        public SideElementType Type;
        public int Capacity;

        public SideElement() { }
    }
}

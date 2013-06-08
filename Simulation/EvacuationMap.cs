using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;

namespace Simulation
{
    public class EvacuationMap
    {
        private EvacuationElement[][] _map;

        public EvacuationElement this[uint row, uint col]
        {
            get
            {
                return _map[row][col];
            }
        }

        public void InitializeFromBuildingMap(BuildingMap bm)
        {
            _map = new EvacuationElement[bm.Height][];
            for (uint i = 0; i < bm.Height; ++i)
            {
                _map[i] = new EvacuationElement[bm.Width];
                for (int j = 0; j < bm.Width; ++j)
                {
                    _map[i][j] = new EvacuationElement(bm.Floor[i][j]);
                }
            }
        }

        public void SetPeopleGroup(PeopleGroup group)
        {
            _map[group.Row][group.Col].PeopleQuantity = group.Quantity;
        }

        public void ResetPeopleGroups()
        {
            foreach (EvacuationElement[] e in _map)
                foreach (EvacuationElement element in e)
                    element.PeopleQuantity = 0;
        }

    }
}

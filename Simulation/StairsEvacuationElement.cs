using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;

namespace Simulation
{
    public class StairsEvacuationElement : EvacuationElement
    {
        private List<KeyValuePair<uint, uint>> _groups;

        private uint _totalPeople;

        private uint _startingDelay;

        public override uint PeopleQuantityLeft
        {
            get
            {
                return FloorSquare.Capacity - _totalPeople;
            }
        }

        public StairsEvacuationElement(StairsEntry se, EvacuationMap em) : base(new Structure.FloorSquare(se.ConnectedStairs.Capacity))
        {
            _startingDelay = se.ConnectedStairs.Delay;
            _groups = new List<KeyValuePair<uint, uint>>();

            DetermineNextStep(se, em);
            Passage = se;
        }

        private void DetermineNextStep(StairsEntry se, EvacuationMap em)
        {
            StairsEntry secondEntry = se.ConnectedStairs.GetEntry(1 - se.ID);
            EvacuationElement tempEe = em.Get(secondEntry.Position);

            if (tempEe != null)
            {
                NextStep = tempEe;
            }
            else
            {
                NextStep = em.Get(secondEntry.Position.FindAdjacentSquare());
            }
        }

        public override void StartProcessing()
        {
            uint newDelay;
            List<KeyValuePair<uint, uint>> newList = new List<KeyValuePair<uint, uint>>();

            base.StartProcessing();
            for (int i = 0; i < _groups.Count; ++i)
            {
                newDelay = _groups[i].Key - 1;
                if (newDelay == 0)
                {
                    PeopleQuantity += _groups[i].Value;
                }
                else
                {
                    newList.Add(new KeyValuePair<uint, uint>(newDelay, _groups[i].Value));
                }                
            }
            _groups = newList;
        }

        /*public override void ResetProcessing()
        {
            base.ResetProcessing();
            _groups.Clear();
            _totalPeople = 0;
        }*/

        public override void AddPeople(uint quantity)
        {
            _totalPeople += quantity;
            _groups.Add(new KeyValuePair<uint, uint>(_startingDelay, quantity));
        }

        public override void RemovePeople(uint quantity)
        {
            base.RemovePeople(quantity);
            _totalPeople -= quantity;
        }

        public override bool ContainsPeople()
        {
            return _totalPeople != 0;
        }


    }
}

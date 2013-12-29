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
        private List<KeyValuePair<int, int>> _groups;

        private int _totalPeople;

        private int _startingDelay;

        public override int PeopleQuantityLeft
        {
            get
            {
                return FloorSquare.Capacity - _totalPeople;
            }
        }

        public StairsEvacuationElement(StairsEntry se, EvacuationMap em) : base(new Structure.Tile(se.ConnectedStairs.Capacity))
        {
            _startingDelay = se.ConnectedStairs.Delay;
            _groups = new List<KeyValuePair<int, int>>();

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
                NextStep = em.Get(secondEntry.Position.GetAdjacentPosition());
            }
        }

        public override void StartProcessing()
        {
            int newDelay;
            List<KeyValuePair<int, int>> newList = new List<KeyValuePair<int, int>>();

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
                    newList.Add(new KeyValuePair<int, int>(newDelay, _groups[i].Value));
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

        public override void AddPeople(int quantity)
        {
            _totalPeople += quantity;
            _groups.Add(new KeyValuePair<int, int>(_startingDelay, quantity));
        }

        public override void RemovePeople(int quantity)
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

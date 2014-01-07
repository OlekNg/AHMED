using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;

namespace Simulation
{
    /// <summary>
    /// Special evcuation element used to manage stairs
    /// </summary>
    public class StairsEvacuationElement : EvacuationElement
    {
        /// <summary>
        /// List of people groups moving in stairs
        /// </summary>
        private List<KeyValuePair<int, int>> _groups;

        /// <summary>
        /// Total number of people using this stairs
        /// </summary>
        private int _totalPeople;

        /// <summary>
        /// Starting delay for each getting in people group
        /// </summary>
        private int _startingDelay;

        /// <summary>
        /// Left place for other people
        /// </summary>
        public override int PeopleQuantityLeft
        {
            get
            {
                return Capacity - _totalPeople;
            }
        }

        /// <summary>
        /// Counstrructor - initializes delay, passage and determine next step (exit from stairs)
        /// </summary>
        /// <param name="se">Stairs entry</param>
        /// <param name="em">Whole evacuation map</param>
        public StairsEvacuationElement(StairsEntry se, EvacuationMap em) : base(se.ConnectedStairs.Capacity)
        {
            _startingDelay = se.ConnectedStairs.Delay;
            _groups = new List<KeyValuePair<int, int>>();

            DetermineNextStep(se, em);
        }

        /// <summary>
        /// Setup next step (filed connected with other stairs entry)
        /// </summary>
        /// <param name="se">First stairs entry</param>
        /// <param name="em">Whole evacution map</param>
        private void DetermineNextStep(StairsEntry se, EvacuationMap em)
        {
            StairsEntry secondEntry = se.ConnectedStairs.GetEntry(1 - se.ID);
            EvacuationElement tempEe = em.Get(secondEntry.Position);
            Passage = secondEntry;
            if (tempEe != null)
            {
                NextStep = tempEe;
            }
            else
            {
                NextStep = em.Get(secondEntry.Position.GetAdjacentPosition());
            }
        }

        /// <summary>
        /// Method called at the beggining of the file processing. Index for each people group is decremented. If it equals 0, that people group is read to get out.
        /// </summary>
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

        /// <summary>
        /// Add people to stairs - new poeple group is added to list with index equal to _startingDelay
        /// </summary>
        /// <param name="quantity">Quantity of people group</param>
        public override void AddPeople(int quantity)
        {
            _totalPeople += quantity;
            _groups.Add(new KeyValuePair<int, int>(_startingDelay, quantity));
        }

        /// <summary>
        /// Remove poeple from stairs
        /// </summary>
        /// <param name="quantity">People group quantity</param>
        public override void RemovePeople(int quantity)
        {
            base.RemovePeople(quantity);
            _totalPeople -= quantity;
        }

        /// <summary>
        /// Check if there is anybody on stairs
        /// </summary>
        /// <returns>Is there anybody out there?</returns>
        public override bool ContainsPeople()
        {
            return _totalPeople != 0;
        }


    }
}

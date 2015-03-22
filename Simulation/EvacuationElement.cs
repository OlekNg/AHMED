using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structure;

namespace Simulation
{
    /// <summary>
    /// Simple element constructing each evacuation route
    /// </summary>
    public class EvacuationElement
    {
        /// <summary>
        /// Capacity of this element
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Type of passage to next step in evacuation route
        /// </summary>
        public IWallElement Passage { get; set; }

        /// <summary>
        /// Number of people standing in this element
        /// </summary>
        public int PeopleQuantity { get; protected set; }

        /// <summary>
        /// Was this element processed in this cycle?
        /// </summary>
        public bool Processed { get; set; }

        /// <summary>
        /// Is there any path that leads to exit from this evacuation element.
        /// </summary>
        public bool ExistsPathToExit { get; set; }

        /// <summary>
        /// Next step in evacuation route 
        /// </summary>
        public EvacuationElement NextStep { get; set; }

        /// <summary>
        /// All (four) evacuation elements contiguous with this one 
        /// </summary>
        public EvacuationElement[] Neighbours { get; set; }

        /// <summary>
        /// Passages to all (four) evacuation elements contiguous with this one
        /// </summary>
        public IWallElement[] NeighboursPassages { get; set; }

        /// <summary>
        /// How many people can get in this evacuation route element
        /// </summary>
        public virtual int PeopleQuantityLeft { 
            get 
            { 
                return Capacity - PeopleQuantity; 
            } 
        }

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="c">Capacity of this field</param>
        public EvacuationElement(int c)
        {
            Processed = false;
            Capacity = c;
            Neighbours = new EvacuationElement[4];
            NeighboursPassages = new IWallElement[4];
        }

        /// <summary>
        /// Method called before end of each simulation cycle - it sets this field as unprocessed
        /// </summary>
        public virtual void ResetProcessing()
        {
            Processed = false;
        }

        /// <summary>
        /// Method called after very beginning of processing this element - it setd this element as processed
        /// </summary>
        public virtual void StartProcessing()
        {
            Processed = true;
        }

        /// <summary>
        /// Move people into that field
        /// </summary>
        /// <param name="quantity">Quantity of the people group</param>
        public virtual void AddPeople(int quantity)
        {
            PeopleQuantity += quantity;
        }

        /// <summary>
        /// Remove people from this field
        /// </summary>
        /// <param name="quantity">Quantity of the moving people group</param>
        public virtual void RemovePeople(int quantity)
        {
            PeopleQuantity -= quantity;
        }

        /// <summary>
        /// Method called during initialization of each simulation. Sets field as unprocecessed and poeaple quantity accordingly to parameter
        /// </summary>
        /// <param name="peopleQuantity">Initial people quatity (based on people map)</param>
        public virtual void Setup(int peopleQuantity)
        {
            Processed = false;
            PeopleQuantity = peopleQuantity;
        }

        /// <summary>
        /// Method used to check if there are people standing on this element
        /// </summary>
        /// <returns>Is there any people?</returns>
        public virtual bool ContainsPeople()
        {
            return PeopleQuantity != 0;
        }

        /// <summary>
        /// Goes backward based on NextStep to get possible evacuation elements with people.
        /// </summary>
        public virtual IEnumerable<EvacuationElement> GetPossibleEvaucationGroups(bool excludeStairs)
        {
            if (ContainsPeople())
                yield return this;

            var groups = Neighbours.Where(x => x != null && x.NextStep == this || (!excludeStairs && x is StairsEvacuationElement))
                .SelectMany(x => x.GetPossibleEvaucationGroups(false));

            foreach (var g in groups)
                yield return g;
        }
    }
}

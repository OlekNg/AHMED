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
        /// Floor tile 
        /// </summary>
        public FloorSquare FloorSquare { get; set; }

        /// <summary>
        /// Type of passage to next step in evacuation route
        /// </summary>
        public IWallElement Passage { get; set; }

        /// <summary>
        /// Number of people standing in this element
        /// </summary>
        public uint PeopleQuantity { get; protected set; }

        /// <summary>
        /// Last update time (calculated in elapsed ticks)
        /// </summary>
        public uint Ticks { get; set; }

        /// <summary>
        /// Was this element processed?
        /// </summary>
        public bool Processed { get; private set; }

        /// <summary>
        /// Next step in evacuation route
        /// </summary>
        public EvacuationElement NextStep { get; set; }

        /// <summary>
        /// How many people can get in this evacuation route element
        /// </summary>
        public virtual uint PeopleQuantityLeft { 
            get 
            { 
                return FloorSquare.Capacity - PeopleQuantity; 
            } 
        }

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="fs">Floor tile connected with this evacuation element</param>
        public EvacuationElement(FloorSquare fs)
        {
            Ticks = 0;
            Processed = false;
            FloorSquare = fs;
        }

        public virtual void ResetProcessing()
        {
            Processed = false;
        }

        public virtual void StartProcessing()
        {
            Processed = true;
        }

        public virtual void AddPeople(uint quantity)
        {
            PeopleQuantity += quantity;
        }

        public virtual void RemovePeople(uint quantity)
        {
            PeopleQuantity -= quantity;
        }

        public virtual void Setup(uint peopleQuantity)
        {
            Processed = false;
            PeopleQuantity = peopleQuantity;
        }

        public virtual bool ContainsPeople()
        {
            return PeopleQuantity != 0;
        }
    }
}

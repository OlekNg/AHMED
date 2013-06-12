using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    /// <summary>
    /// Class describing one people group
    /// </summary>
    public class PeopleGroup
    {
        /// <summary>
        /// Group location - row
        /// </summary>
        public uint Row { get; set; }

        /// <summary>
        /// Group location - column
        /// </summary>
        public uint Col { get; set; }

        /// <summary>
        /// Group quantity
        /// </summary>
        public uint Quantity { get; set; }

        /// <summary>
        /// Initalize all properties
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="quantity">Quantity</param>
        public PeopleGroup(uint row, uint col, uint quantity)
        {
            Row = row;
            Col = col;
            Quantity = quantity;
        }
    }
}

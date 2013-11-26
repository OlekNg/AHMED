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
        public int Floor { get; set; }

        /// <summary>
        /// Group location - row
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Group location - column
        /// </summary>
        public int Col { get; set; }

        /// <summary>
        /// Group quantity
        /// </summary>
        public int Quantity { get; set; }

        

        /// <summary>
        /// Initalize all properties
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="col">Column</param>
        /// <param name="quantity">Quantity</param>
        public PeopleGroup(int floor, int row, int col, int quantity)
        {
            Floor = floor;
            Row = row;
            Col = col;
            Quantity = quantity;
        }
    }
}

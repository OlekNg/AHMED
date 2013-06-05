using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure
{
    public class PeopleMap
    {
        public PeopleGroup[] People;

        public PeopleGroup GetByPosition(uint x, uint y)
        {
            foreach (PeopleGroup group in People)
            {
                if (group.X == x && group.Y == y)
                {
                    return group;
                }
            }
            return null;
        }
    }
}

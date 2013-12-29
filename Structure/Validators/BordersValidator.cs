using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structure.Validators
{
    public class BordersValidator : IFloorSquareValidator
    {

        public ValidatorInfo Validate(int x, int y, Floor f, ValidationResult result)
        {
            Tile[] neighbours = f.GetNeighbours(x, y);
            Tile center = f.Get(x, y);

            for (int i = 0; i < 4; ++i)
            {
                //if (neighbours[i] == null && center.Side[i].Type == WallElementType.STANDARD_PASSAGE)
                    //result.Add(new UnfilledBorderFound());
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DataModel.Enums;

namespace Structure.Validators
{
    public class UnfilledBorderFound : ValidatorInfo
    {
        public ValidatorInfoLevel Level
        {
            get { return ValidatorInfoLevel.ERROR; }
        }

        private Direction _direction;

        private int _floor;

        private int _x;

        private int _y;

        public UnfilledBorderFound(int floor, int x, int y, Direction direction)
        {
            _floor = floor;
            _x = x;
            _y = y;
            _direction = direction;
        }
    }
}

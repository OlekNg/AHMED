using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structure.Validators
{
    public class DefaultValidator : Validator
    {
        public DefaultValidator() : base()
        {
            AddFloorsSquareValidator(new BordersValidator());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structure.Validators
{
    public class ValidationResult
    {
        private IList<ValidatorInfo> _infos;

        public bool StopProcess()
        {
            return _infos.Any(e => e.Level == ValidatorInfoLevel.ERROR);
        }
    }
}

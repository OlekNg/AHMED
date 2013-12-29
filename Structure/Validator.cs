using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structure.Validators;

namespace Structure
{
    public class Validator
    {
        private List<IFloorSquareValidator> _floorSquareValidators;

        public Validator()
        {
            _floorSquareValidators = new List<IFloorSquareValidator>();
        }

        public void AddFloorsSquareValidator(IFloorSquareValidator validator)
        {
            _floorSquareValidators.Add(validator);
        }

        public ValidationResult Validate(BuildingMap bm)
        {
            ValidationResult vr = new ValidationResult();
            /*
            foreach (Floor f in bm.Floors)
                for (int w = 0; w < f.Width; ++w)
                    for (int h = 0; h < f.Height; ++h)
                        foreach (IFloorSquareValidator fsv in _floorSquareValidators)
                            fsv.Validate(w, h, f, vr);

            */
            return vr;
        }
    }
}

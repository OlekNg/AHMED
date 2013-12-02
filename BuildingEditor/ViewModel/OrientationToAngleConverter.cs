using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BuildingEditor.ViewModel
{
    [ValueConversion(typeof(Direction), typeof(int))]
    public class OrientationToAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                Direction side = (Direction)value;
                switch (side)
                {
                    case Direction.LEFT:
                        return 90;
                    case Direction.UP:
                        return 180;
                    case Direction.RIGHT:
                        return 270;
                    default:
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

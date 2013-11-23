using Common.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BuildingEditor.Logic
{
    [ValueConversion(typeof(Side), typeof(int))]
    public class OrientationToAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                Side side = (Side)value;
                switch (side)
                {
                    case Side.LEFT:
                        return 90;
                    case Side.TOP:
                        return 180;
                    case Side.RIGHT:
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Main.StatusControl
{
    [ValueConversion(typeof(double), typeof(string))]
    public class PercentageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double p = (double)value;
                int r = Math.Min(255, (int)(p * 2 * 255));
                int g = Math.Min(255, 255 - (int)((p - 0.5) * 2 * 255));
                int b = 0;

                return String.Format("#{0}{1}{2}", r.ToString("X2"), g.ToString("X2"), b.ToString("X2"));
            }
            catch
            {
                return "#000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

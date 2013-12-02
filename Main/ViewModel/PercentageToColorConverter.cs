using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Main.ViewModel
{
    /// <summary>
    /// Converts percentage to color (from green for 0% through orange to red for 100%).
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class PercentageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                // From 0.0 to 0.5 we are increasing red component (0x00 to 0xFF).
                // Then from 0.5 to 1.0 we are decreasing green component (0xFF to 0x00).
                double p = (double)value;
                int r = Math.Min(255, (int)(p * 2 * 255));
                int g = Math.Min(255, 255 - (int)((p - 0.5) * 2 * 255));
                int b = 0;

                // Convert to format #000000
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

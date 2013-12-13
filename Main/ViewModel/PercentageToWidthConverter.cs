using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Main.ViewModel
{
    /// <summary>
    /// Converts percentage value to width value based on parameter (full width).
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class PercentageToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double p = (double)value;
                int fullWidth = Int32.Parse((string)parameter);
                return (int)(p * fullWidth);
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

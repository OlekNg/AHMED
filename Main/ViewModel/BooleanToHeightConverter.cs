using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Main.ViewModel
{
    /// <summary>
    /// Converts percentage to color (from green for 0% through orange to red for 100%).
    /// </summary>
    [ValueConversion(typeof(Boolean), typeof(string))]
    public class BooleanToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                bool b = (bool)value;
                return b ? "*" : "0";
            }
            catch
            {
                return "*";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

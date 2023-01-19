

using System;
using System.Globalization;
using System.Windows.Data;

namespace Metaforge_Marketing.HelperClasses.Converters
{
    public class OrderTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;
            if (val == 1) { return "One Time"; }
            else if (val == 2) { return "Monthly"; }
            else { return "None"; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = (string)value;
            if (String.Equals(val, "One Time")) { return 1; }
            else if (String.Equals(val, "Monthly")) { return 2; }
            else { return 0; }
        }
    }
}

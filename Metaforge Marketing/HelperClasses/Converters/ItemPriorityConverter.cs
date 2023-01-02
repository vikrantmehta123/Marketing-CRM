using System;
using System.Globalization;
using System.Windows.Data;

namespace Metaforge_Marketing.HelperClasses.Converters
{
    public class ItemPriorityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(int))
            {
                int val = (int)value;
                if (val == 0) { return "High"; }
                else if(val == 1) { return "Moderate"; }
                else { return "Low"; }
            }
            else { return "None"; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(string))
            {
                string val = (string)value;
                if(String.Equals(val, "High")) { return 0; }
                else if(String.Equals(val, "Moderate")) { return 1; }
                else { return "Low"; }
            }
            else { return 2; }
        }
    }
}

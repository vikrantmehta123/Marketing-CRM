
using System;
using System.Globalization;
using System.Windows.Data;

namespace Metaforge_Marketing.HelperClasses.Converters
{
    internal class CustomerCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(int))
            {
                int val = (int)value;
                if (val == 0) { return "None"; }
                else if(val == 1) { return "Automotive"; }
                else if(val == 2) { return "Construction"; }
                else if (val == 3) { return "Electrical"; }
                else if (val == 4) { return "Furniture"; }
                else { return "Others"; }
            }
            else { return "None"; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(string))
            {
                string val = (string)value;
                if (String.Equals(val, "None")) { return 0; }
                else if (String.Equals(val, "Automotive")) { return 1; }
                else if (String.Equals(val, "Construction")) { return 2; }
                else if (String.Equals(val, "Electrical")) { return 3; }
                else if(String.Equals(val, "Furniture")) { return 4; }
                else { return 5; }
            }
            else { return 0; }
        }
    }
}

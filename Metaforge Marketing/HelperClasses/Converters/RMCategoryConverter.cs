
using System;
using System.Globalization;
using System.Windows.Data;

namespace Metaforge_Marketing.HelperClasses.Converters
{
    internal class RMCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(int))
            {
                int val = (int)value;
                if (val == 1) { return "Annealed"; }
                else { return "PPD"; }
            }
            else
            {
                return "None";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(string))
            {
                string val = (string)value;
                if (String.Equals(val, "Annealed")) { return 1; }
                else { return 2; }
            }
            else { return 0; }
        }
    }
}

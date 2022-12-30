
using System;
using System.Globalization;
using System.Windows.Data;

namespace Metaforge_Marketing.HelperClasses.Converters
{
    public class IntegerToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(int))
            {
                int val = (int)value;
                if (val == 0) { return "No"; }
                else { return "Yes"; }
            }
            
            else { return "No"; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(string))
            {
                string val = (string)value;
                val = val.ToUpper();
                if (val == "YES") { return 1; }
                else { return 0; }
            }
            else if (value.GetType() == typeof(bool))
            {
                bool val = (bool)value;
                if (val) { return 1; }
                else { return 0; }
            }
            else
            {

                return 0;
            }
        }
    }
}

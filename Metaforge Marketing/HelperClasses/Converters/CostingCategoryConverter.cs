
using System;
using System.Globalization;
using System.Windows.Data;

namespace Metaforge_Marketing.HelperClasses.Converters
{
    public class CostingCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(int))
            {
                int val = (int)value;
                if (val == 1) { return "Metaforge"; }
                else if (val == 2) { return "Customer Quoted"; }
                else if (val == 3) { return "Customer Approved"; }
                else { return "None"; }
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
                if (String.Equals(val, "Metaforge")) { return 1; }
                else if (String.Equals(val, "Customer Quoted")) { return 2; }
                else if (String.Equals(val, "Csutomer Approved")) { return 3; }
                else { return 0; }
            }
            else
            {
                return 0;
            }
            
        }
    }
}

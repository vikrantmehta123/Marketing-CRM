
using System;
using System.Globalization;
using System.Windows.Data;

namespace Metaforge_Marketing.HelperClasses.Converters
{
    public class CostingCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;
            if (val == 1) { return "Metaforge"; }
            else if (val == 2) { return "Customer Quoted"; }
            else if(val == 3) { return "Customer Approved"; }
            else { return "None"; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = (string)value;
            if (String.Equals(val, "Metaforge")) { return 1; }
            else if (String.Equals(val, "Customer Quoted")) { return 2; }
            else if (String.Equals(val, "Csutomer Approved")) { return 3; }
            else { return 0; }
        }
    }
}

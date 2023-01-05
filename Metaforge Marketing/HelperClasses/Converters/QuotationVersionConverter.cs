
using System;
using System.Globalization;
using System.Windows.Data;

namespace Metaforge_Marketing.HelperClasses.Converters
{
    public class QuotationVersionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null) return null;
            if(value.GetType() == typeof(int))
            {
                int val = (int)value;
                if(val == -1) { return "Metaforge"; }
                else if(val == 0) { return "Customer Draft"; }
                else { return $"V{val}"; }
            }
            else { return "NA"; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if(value.GetType() == typeof(string))
            {
                string val = (string)value;
                if(String.Equals(val, "Metaforge")) { return -1; }
                else if(String.Equals(val, "Customer Draft")) { return 0; }
                else { return System.Convert.ToInt16(val); }
            }
            return null;
        }
    }
}

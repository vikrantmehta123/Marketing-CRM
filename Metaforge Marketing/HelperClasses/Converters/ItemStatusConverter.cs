using System;
using System.Globalization;
using System.Windows.Data;

namespace Metaforge_Marketing.HelperClasses.Converters
{
    internal class ItemStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int status = (int)value;
            switch (status)
            {
                case 0:
                    return "Pending";
                case 1:
                    return "Regretted";
                case 2:
                    return "Rejected By Customer";
                case 3:
                    return "MF Costing Prepared";
                case 4:
                    return "Customer Costing Prepared";
                case 5:
                    return "Quotation Sent";
                case 6:
                    return "PO Received";

                default: return "Pending";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch(value.ToString())
            {
                case "Pending": return 0;
                case "Regretted": return 1;
                case "Rejected By Customer": return 2;
                case "MF Costing Prepared": return 3;
                case "Customer Costing Prepared": return 4;
                case "Quotation Sent": return 5;
                case "PO Received": return 6;
                default: return 0;
            }
        }
    }
}

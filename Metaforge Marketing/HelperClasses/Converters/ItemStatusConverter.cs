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
                case -1:
                    return "None";
                case 0:
                    return "Pending";
                case 1:
                    return "Regretted";
                case 2:
                    return "MF Costing Prepared";
                case 3:
                    return "Customer Costing Prepared";
                case 4:
                    return "Quotation Sent";
                case 5:
                    return "Regret Mail Sent";
                case 6:
                    return "PO Received";
                case 7:
                    return "Rejected By Customer";
                default: return "Pending";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch(value.ToString())
            {
                case "None": return -1;
                case "Pending": return 0;
                case "Regretted": return 1;
                case "MF Costing Prepared": return 2;
                case "Customer Costing Prepared": return 3;
                case "Quotation Sent": return 4;
                case "Regret Mail Sent": return 5;
                case "PO Received": return 6;
                case "Rejected By Customer": return 7;
                default: return 0;
            }
        }
    }
}

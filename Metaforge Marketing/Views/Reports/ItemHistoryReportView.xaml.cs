using System.Windows.Controls;

namespace Metaforge_Marketing.Views.Reports
{
    public partial class ItemHistoryReportView : UserControl
    {
        public ItemHistoryReportView()
        {
            DataContext = new ViewModels.Reports.ItemHistoryReportViewModel();
            InitializeComponent();
        }
    }
}

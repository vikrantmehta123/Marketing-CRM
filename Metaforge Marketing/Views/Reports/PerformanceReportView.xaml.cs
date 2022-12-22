using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Reports
{
    public partial class PerformanceReportView : UserControl
    {
        public PerformanceReportView()
        {
            DataContext = new ViewModels.Reports.PerformanceReportViewModel();
            InitializeComponent();
        }
    }
}

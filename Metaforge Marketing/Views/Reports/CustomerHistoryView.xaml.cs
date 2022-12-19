using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Reports
{
    public partial class CustomerHistoryView : UserControl
    {
        public CustomerHistoryView()
        {
            DataContext = new ViewModels.Reports.CustomerHistoryViewModel();
            InitializeComponent();
        }
    }
}

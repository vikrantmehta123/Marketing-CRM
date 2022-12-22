using System.Windows.Controls;


namespace Metaforge_Marketing.Views.RFQs
{
    public partial class CostingView : UserControl
    {
        public CostingView()
        {
            DataContext = new ViewModels.RFQs.CostingViewModel();
            InitializeComponent();
        }
    }
}

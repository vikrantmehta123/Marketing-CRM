using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Costing
{
    public partial class CustomerCostingView : UserControl
    {
        public CustomerCostingView()
        {
            DataContext = new ViewModels.Costing.CustomerCostingViewModel();
            InitializeComponent();
        }
    }
}

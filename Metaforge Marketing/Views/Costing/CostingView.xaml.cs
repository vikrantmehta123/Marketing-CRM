using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Costing
{
    public partial class CostingView : UserControl
    {
        public CostingView()
        {
            DataContext = new ViewModels.Costing.CostingViewModel();
            InitializeComponent();
        }
    }
}

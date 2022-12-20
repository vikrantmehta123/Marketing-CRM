using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Costing
{
    public partial class MFCostingView : UserControl
    {
        public MFCostingView()
        {
            DataContext = new ViewModels.Costing.MFCostingViewModel();
            InitializeComponent();
        }
    }
}

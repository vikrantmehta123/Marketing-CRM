using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Masters
{
    public partial class BuyerMasterView : UserControl
    {
        public BuyerMasterView()
        {
            DataContext = new ViewModels.Masters.BuyerMasterViewModel();
            InitializeComponent();
        }
    }
}

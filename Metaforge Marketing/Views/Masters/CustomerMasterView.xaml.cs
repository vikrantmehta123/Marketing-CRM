using System.Windows.Controls;

namespace Metaforge_Marketing.Views.Masters
{
    public partial class CustomerMasterView : UserControl
    {
        public CustomerMasterView()
        {
            DataContext = new ViewModels.Masters.CustomerMasterViewModel();
            InitializeComponent();
        }
    }
}

using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Shared
{
    public partial class SelectCustomerView : UserControl
    {
        public SelectCustomerView()
        {
            DataContext = new ViewModels.Shared.SelectCustomerViewModel();
            InitializeComponent();
        }
    }
}

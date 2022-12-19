using System.Windows.Controls;

namespace Metaforge_Marketing.Views.Add
{
    public partial class AddCustomerView : UserControl
    {
        public AddCustomerView()
        {
            DataContext = new ViewModels.Add.AddCustomerViewModel();
            InitializeComponent();
        }
    }
}

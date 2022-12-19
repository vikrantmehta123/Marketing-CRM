using System.Windows.Controls;

namespace Metaforge_Marketing.Views.Add
{
    public partial class AddBuyerView : UserControl
    {
        public AddBuyerView()
        {
            DataContext = new ViewModels.Add.AddBuyerViewModel();
            InitializeComponent();
        }
    }
}

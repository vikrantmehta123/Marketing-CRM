using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Add
{
    public partial class AddRMMasterDataView : UserControl
    {
        public AddRMMasterDataView()
        {
            DataContext = new ViewModels.Add.AddRMMasterDataViewModel();
            InitializeComponent();
        }
    }
}

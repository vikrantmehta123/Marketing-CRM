using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Masters
{
    public partial class ItemMasterView : UserControl
    {
        public ItemMasterView()
        {
            DataContext = new ViewModels.Masters.ItemMasterViewModel();
            InitializeComponent();
        }
    }
}

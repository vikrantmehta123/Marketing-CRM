using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Masters
{
    public partial class AdminMasterView : UserControl
    {
        public AdminMasterView()
        {
            DataContext = new ViewModels.Masters.AdminMasterViewModel();
            InitializeComponent();
        }
    }
}

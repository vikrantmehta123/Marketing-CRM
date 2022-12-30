using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Masters
{
    public partial class OperationsMasterView : UserControl
    {
        public OperationsMasterView()
        {
            DataContext = new ViewModels.Masters.OperationsMasterViewModel();
            InitializeComponent();
        }
    }
}

using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Masters
{
    public partial class UpdateRMMasterView : UserControl
    {
        public UpdateRMMasterView()
        {
            DataContext = new ViewModels.Masters.UpdateRMMasterViewModel();
            InitializeComponent();
        }
    }
}

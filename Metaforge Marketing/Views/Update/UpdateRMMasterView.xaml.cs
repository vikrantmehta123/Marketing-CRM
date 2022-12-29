using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Update
{
    public partial class UpdateRMMasterView : UserControl
    {
        public UpdateRMMasterView()
        {
            DataContext = new ViewModels.Update.UpdateRMMasterViewModel();
            InitializeComponent();
        }
    }
}

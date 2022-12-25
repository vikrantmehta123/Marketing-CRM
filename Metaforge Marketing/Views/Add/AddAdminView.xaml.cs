using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Add
{
    public partial class AddAdminView : UserControl
    {
        public AddAdminView()
        {
            DataContext = new ViewModels.Add.AddAdminViewModel();
            InitializeComponent();
        }
    }
}

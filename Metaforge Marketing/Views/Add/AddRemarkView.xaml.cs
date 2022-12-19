using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Add
{
    public partial class AddRemarkView : UserControl
    {
        public AddRemarkView()
        {
            DataContext = new ViewModels.Add.AddRemarkViewModel();
            InitializeComponent();
        }
    }
}

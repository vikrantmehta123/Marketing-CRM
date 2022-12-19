using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Add
{
    public partial class AddRFQView : UserControl
    {
        public AddRFQView()
        {
            DataContext = new ViewModels.Add.AddRFQViewModel();
            InitializeComponent();
        }
    }
}

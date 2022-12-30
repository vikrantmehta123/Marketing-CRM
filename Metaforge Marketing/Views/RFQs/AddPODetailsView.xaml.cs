using System.Windows.Controls;


namespace Metaforge_Marketing.Views.RFQs
{
    public partial class AddPODetailsView : UserControl
    {
        public AddPODetailsView()
        {
            DataContext = new ViewModels.RFQs.AddPODetailsViewModel();
            InitializeComponent();
        }
    }
}

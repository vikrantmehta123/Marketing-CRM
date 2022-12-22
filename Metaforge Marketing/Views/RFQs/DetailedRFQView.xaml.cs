using System.Windows.Controls;


namespace Metaforge_Marketing.Views.RFQs
{
    public partial class DetailedRFQView : UserControl
    {
        public DetailedRFQView()
        {
            DataContext = new ViewModels.RFQs.DetailedRFQViewModel();
            InitializeComponent();
        }
    }
}

using System.Windows.Controls;


namespace Metaforge_Marketing.Views.RFQs.SentQuotes
{
    public partial class ShowDetailsView : UserControl
    {
        public ShowDetailsView()
        {
            DataContext = new ViewModels.RFQs.SentQuoteViewModels.ShowDetailsViewModel();
            InitializeComponent();
        }
    }
}

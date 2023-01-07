using System.Windows.Controls;

namespace Metaforge_Marketing.Views.RFQs.SentQuotes
{
    public partial class SentQuotationsView : UserControl
    {
        public SentQuotationsView()
        {
            DataContext = new ViewModels.RFQs.SentQuoteViewModels.SentQuotationsViewModel();
            InitializeComponent();
        }
    }
}

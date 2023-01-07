

using Metaforge_Marketing.Models;

namespace Metaforge_Marketing.ViewModels.RFQs.SentQuoteViewModels
{
    public abstract class SentQuotationsContainer : ViewModelBase
    {
        public static RFQ SelectedRFQ { get; set; } = new RFQ();
    }
}

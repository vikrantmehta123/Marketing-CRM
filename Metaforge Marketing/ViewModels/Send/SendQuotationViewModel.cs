
using Metaforge_Marketing.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Send
{
    public class SendQuotationViewModel : ViewModelBase
    {
        #region Fields
        private ICommand _sendQuoteCommand;
        private ObservableCollection<RFQ> _rfqs;
        #endregion Fields

        #region Properties
        public ObservableCollection<RFQ> RFQs
        {
            get
            {
                return _rfqs;
            }
        }

        public ICommand SendQuoteCommand
        {
            get
            {
                return _sendQuoteCommand;
            }
        }

        #endregion Properties
    }
}

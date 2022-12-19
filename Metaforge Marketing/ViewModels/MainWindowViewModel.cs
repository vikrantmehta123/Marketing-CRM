
using Metaforge_Marketing.HelperClasses;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string WindowTitle { get; } = "Home Page:";

        #region Fields
        private ICommand _addCustomerCommand, _addBuyerCommand, _addRFQCommand, _addRemarkCommand;
        private ICommand _addMFCostingCommand, _addCustomerCostingCommand;
        private ICommand _customerHistoryCommand, _costingComparisonCommand, _rfqDetailsCommand;
        private ICommand _sendGeneralMailCommand, _sendQuotationMailCommand;
        #endregion Fields

        #region Commands
        public ICommand AddCustomerCommand
        {
            get
            {
                if (_addCustomerCommand== null)
                {
                    _addCustomerCommand = new Command(p => ChangeViewModel(new ViewModels.Add.AddCustomerViewModel()));
                }
                return _addCustomerCommand;
            }
        }

        public ICommand AddBuyerCommand
        {
            get
            {
                if (_addBuyerCommand == null)
                {
                    _addBuyerCommand = new Command(p => ChangeViewModel(new ViewModels.Add.AddBuyerViewModel()));
                }
                return _addBuyerCommand;
            }
        }

        public ICommand AddRFQCommand
        {
            get
            {
                if (_addRFQCommand == null)
                {
                    _addRFQCommand = new Command(p => ChangeViewModel(new Add.AddRFQViewModel()));
                }
                return _addRFQCommand;
            }
        }
        public ICommand AddRemarkCommand
        {
            get
            {
                if (_addRemarkCommand == null)
                {
                    _addRemarkCommand = new Command(p => ChangeViewModel(new Add.AddRemarkViewModel()));
                }
                return _addRemarkCommand;
            }
        }

        public ICommand AddMFCostingCommand
        {
            get
            {
                if (_addMFCostingCommand == null)
                {
                    _addMFCostingCommand = new Command(p => ChangeViewModel(new Costing.MFCostingViewModel()));
                }
                return _addMFCostingCommand;
            }
        }
        public ICommand AddCustomerCostingCommand
        {
            get
            {
                if (_addCustomerCostingCommand == null)
                {
                    _addCustomerCostingCommand = new Command(p => ChangeViewModel(new Costing.CustomerCostingViewModel()));
                }
                return _addCustomerCostingCommand;
            }
        }
        public ICommand RFQDetailsCommand
        {
            get
            {
                if (_rfqDetailsCommand == null)
                {
                    
                }
                return _rfqDetailsCommand;
            }
        }

        public ICommand CostingComparisonCommand
        {
            get
            {
                if (_costingComparisonCommand== null)
                {
                    _costingComparisonCommand = new Command(p => ChangeViewModel(new Reports.CostingComparisonViewModel()));
                }
                return _costingComparisonCommand;
            }
        }

        public ICommand CustomerHistoryCommand
        {
            get
            {
                if (_customerHistoryCommand == null)
                {
                    _customerHistoryCommand = new Command(p => ChangeViewModel(new Reports.CustomerHistoryViewModel()));
                }
                return _customerHistoryCommand; 
            }
        }

        public ICommand SendGeneralMailCommand
        {
            get
            {
                if (_sendGeneralMailCommand == null)
                {
                    _sendGeneralMailCommand = new Command(p => ChangeViewModel(new Send.SendGeneralMailViewModel()));
                }
                return _sendGeneralMailCommand;
            }
        }

        public ICommand SendQuotationMailCommand
        {
            get
            {
                if (_sendQuotationMailCommand == null)
                {
                    _sendQuotationMailCommand = new Command(p => ChangeViewModel(new Send.SendQuotationMailViewModel()));
                }
                return _sendQuotationMailCommand;
            }
        }

        #endregion Commands
    }
}

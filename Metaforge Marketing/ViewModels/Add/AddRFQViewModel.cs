

using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.ViewModels.Shared;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Add
{
    public class AddRFQViewModel : SharedViewModelBase
    {
        #region Fields
        private ICommand _selectCustomerCommand, _addAnotherItemCommand, _clearCommand, _saveCommand;
        #endregion Fields


        #region Properties
        public RFQ RFQToAdd { get; set; } = new RFQ { Customer = new Customer()};
        public Item RFQItem { get; set; } = new Item();

        #endregion Properties

        #region Commands
        public ICommand SelectCustomerCommand
        {
            get
            {
                if (_selectCustomerCommand == null)
                {
                    _selectCustomerCommand = new Command(p=> new PopupWindowViewModel().Show(new SelectCustomerViewModel()));
                }
                return _selectCustomerCommand;
            }
        }

        public ICommand AddAnotherItemCommand
        {
            get
            {
                if(_addAnotherItemCommand == null)
                {
                    _addAnotherItemCommand = new Command(p => AddAnotherItem());
                }
                return _addAnotherItemCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new Command(p=> Clear());
                }
                return _clearCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new Command(p=> Save());
                }
                return _saveCommand;
            }
        }
        #endregion Commands


        #region Methods
        private void AddAnotherItem()
        {
            RFQToAdd.Items.Add( RFQItem );
            RFQItem = new Item();
            OnPropertyChanged( nameof( RFQItem ) );
        }
        private void Save()
        {
            RFQToAdd.Items.Add(RFQItem);
            if (RFQToAdd.Customer != null)
            {
                SQLWrapper<RFQ>.InsertWrapper(Repository.RFQsRepository.InsertToDB, RFQToAdd);
            }
            RFQItem = new Item();
            OnPropertyChanged(nameof(RFQItem));
        }
        private void Clear() { SelectedRFQ = null; }

        #endregion Methods
        public AddRFQViewModel() 
        {
            StaticPropertyChanged += AddRFQViewModel_StaticPropertyChanged;
        }

        private void AddRFQViewModel_StaticPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedCustomer))
            {
                RFQToAdd.Customer = SelectedCustomer;
            }
        }
    }
}


using Metaforge_Marketing.Repository;
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.ViewModels.Shared;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System;

namespace Metaforge_Marketing.ViewModels.Add
{
    public class AddRFQViewModel : SharedViewModelBase
    {
        #region Fields
        private ICommand _selectCustomerCommand, _selectBuyerCommand, _addAnotherItemCommand, _clearCommand, _saveCommand;
        private ObservableCollection<Admin> _admins;
        #endregion Fields


        #region Properties
        public RFQ RFQToAdd { get; set; } = new RFQ { Customer = new Customer()};
        public Item RFQItem { get; set; } = new Item();
        public ObservableCollection<Admin> Admins
        {
            get
            {
                if (_admins== null)
                {
                    _admins = new ObservableCollection<Admin>(SQLWrapper<Admin>.FetchWrapper(AdminsRepository.FetchAdmins));
                }
                return _admins;
            }
        }

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

        public ICommand SelectBuyerCommand
        {
            get
            {
                if(_selectBuyerCommand == null && SelectedCustomer != null)
                {
                    _selectBuyerCommand = new Command(p => new PopupWindowViewModel().Show(new SelectBuyerViewModel(SelectedCustomer)));
                }
                return _selectBuyerCommand;
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
            if (RFQItem.IsFormEntryValid()) { RFQToAdd.Items.Add(RFQItem); }
            if (RFQToAdd.Customer != null && RFQToAdd.Buyer != null)
            {
                try
                {
                    SQLWrapper<RFQ>.InsertWrapper(RFQsRepository.InsertToDB, RFQToAdd);
                    RFQItem = new Item();
                    RFQToAdd = new RFQ() { Customer = new Customer()};
                    RFQToAdd.Items.Clear();
                    OnPropertyChanged(nameof(RFQToAdd));
                    OnPropertyChanged(nameof(RFQItem));
                    OnPropertyChanged(nameof(SelectedCustomer));
                    OnPropertyChanged(nameof(SelectedBuyer));
                    MessageBox.Show("Successfully Added");
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        private void Clear() { 
            RFQToAdd = new RFQ(); 
            RFQItem = new Item();
            SelectedCustomer = null;
            SelectedBuyer = null;
        }

        #endregion Methods
        public AddRFQViewModel() 
        {
            ClearAllSelections(); // Clear All previous selections, if there are any
            StaticPropertyChanged += AddRFQViewModel_StaticPropertyChanged;
        }

        private void AddRFQViewModel_StaticPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedCustomer))
            {
                RFQToAdd.Customer = SelectedCustomer;
                OnPropertyChanged(nameof(SelectBuyerCommand));
            }
            if (e.PropertyName== nameof(SelectedBuyer))
            {
                RFQToAdd.Buyer = SelectedBuyer;
            }
        }
    }
}

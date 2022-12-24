using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.HelperClasses.Commands;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Add
{
    public class AddCustomerViewModel : ViewModelBase
    {
        #region Fields
        private Customer _customerToAdd;
        private Buyer _buyerToAdd;
        private ICommand _addBuyersCommand, _clearCommand, _addAnotherBuyerCommand, _saveCommand;
        private bool _isAddingBuyers, _isAddingCustomer = true;
        #endregion Fields


        #region Properties
        public Buyer BuyerToAdd { get { return _buyerToAdd; } set { _buyerToAdd = value; } }
        public Customer CustomerToAdd { get { return _customerToAdd; } set { _customerToAdd = value; } }

        public bool IsAddingBuyers
        {
            get { return _isAddingBuyers;}
        }

        public bool IsAddingCustomer
        {
            get { return _isAddingCustomer; }
        }

        public ICommand AddBuyersCommand
        {
            get
            {
                return new Command(p => { 
                    if (_isAddingBuyers) { _isAddingBuyers = false; _isAddingCustomer = true; }
                    else { 
                        _isAddingBuyers = true;
                        BuyerToAdd = new Buyer();
                        _customerToAdd.Buyers = new List<Buyer>();
                        _isAddingCustomer = false;
                    }
                    OnPropertyChanged(nameof(IsAddingBuyers));
                    OnPropertyChanged(nameof(IsAddingCustomer));
                } );
            }
        }
        public ICommand ShowCustomerFormCommand
        {
            get
            {
                return new Command(p => { 
                    if (_isAddingBuyers) { _isAddingBuyers = false; _isAddingCustomer = true; }
                    OnPropertyChanged(nameof(IsAddingCustomer));
                    OnPropertyChanged(nameof(IsAddingBuyers));
                } );
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new Command(p => { 
                        CustomerToAdd = new Customer();
                        BuyerToAdd = new Buyer();
                        CustomerToAdd.Buyers.Clear();
                        OnPropertyChanged(nameof(BuyerToAdd));
                        OnPropertyChanged(nameof(CustomerToAdd)); 

                    });
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
                    _saveCommand = new Command(p => Save());
                }
                return _saveCommand;
            }
        }

        public ICommand AddAnotherBuyerCommand
        {
            get
            {
                if (_addAnotherBuyerCommand == null)
                {
                    _addAnotherBuyerCommand = new Command(p => AddAnotherBuyer());
                }
                return _addAnotherBuyerCommand;
            }
        }
        #endregion Properties

        public AddCustomerViewModel()
        {
            _customerToAdd= new Customer();
        }


        #region Command Functions
        private void AddAnotherBuyer()
        {
            _customerToAdd.Buyers.Add(BuyerToAdd);
            BuyerToAdd = new Buyer();
            OnPropertyChanged(nameof(BuyerToAdd));
        }

        private void Save()
        {
            _customerToAdd.Buyers.Add(BuyerToAdd);
            using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                conn.Open();
                CustomersRepository.InsertToDB(conn, _customerToAdd);
                conn.Close();
            }
        }
        #endregion Command Functions
    }
}

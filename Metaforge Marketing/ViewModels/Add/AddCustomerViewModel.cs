using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Add
{
    public class AddCustomerViewModel : ViewModelBase
    {
        #region Fields
        private Customer _customerToAdd = new Customer();
        private List<Buyer> _buyers = new List<Buyer>();
        private Buyer _buyerToAdd = new Buyer();
        private ICommand _clearCommand, _addAnotherBuyerCommand, _saveCommand;
        #endregion Fields


        #region Properties
        public Buyer BuyerToAdd 
        { 
            get { return _buyerToAdd; } 
            set 
            { 
                if (_buyerToAdd != value)
                {
                    _buyerToAdd = value;
                    OnPropertyChanged(nameof(BuyerToAdd));
                }
            } 
        }
        public Customer CustomerToAdd
        {
            get { return _customerToAdd; } 
            set 
            {   
                if(_customerToAdd != value) 
                {
                    _customerToAdd = value;
                    OnPropertyChanged(nameof(CustomerToAdd));
                }
            } 
        }

        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new Command(p => Clear());
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
                    _saveCommand = new Command(p => Save(), p => CanSave());
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
                    _addAnotherBuyerCommand = new Command(p => AddAnotherBuyer(), p => CanAddAnotherBuyer());
                }
                return _addAnotherBuyerCommand;
            }
        }
        #endregion Properties

        #region Methods
        private void Clear()
        {
            BuyerToAdd = new Buyer(); CustomerToAdd = new Customer();
            _buyers.Clear();
        }
        private void AddAnotherBuyer()
        {
            _buyers.Add(BuyerToAdd);
            BuyerToAdd = new Buyer();
        }
        private bool CanAddAnotherBuyer()
        {
            return BuyerToAdd.IsFormDataValid();
        }

        // Summary:
        //      Calls the database save function
        private void Save()
        {
            if(BuyerToAdd.IsFormDataValid()) { _buyers.Add(BuyerToAdd); }
            using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                conn.Open();
                CustomersRepository.InsertToDB(conn, _customerToAdd, _buyers);
                conn.Close();
            }
            Clear();
        }

        // Summary:
        //      Returns true if the Customer's Form data is valid
        private bool CanSave()
        {
            return CustomerToAdd.IsFormDataValid();
        }
        #endregion Methods
    }
}

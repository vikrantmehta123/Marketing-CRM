
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Add
{
    public class AddBuyerViewModel : SharedViewModelBase
    {
        #region Fields
        private Buyer _buyerToAdd;
        private ICommand _addAnotherBuyerCommand, _clearCommand, _selectCustomerCommand, _saveCommand;

        #endregion Fields

        #region Bounded Properties
        public Buyer BuyerToAdd 
        {
            get { return _buyerToAdd; }
            set 
            { 
                _buyerToAdd = value; 
                OnPropertyChanged(nameof(BuyerToAdd));
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
        public ICommand ClearCommand
        {
            get
            {
                if(_clearCommand == null)
                {
                    _clearCommand = new Command(p => Clear());
                }
                return _clearCommand;
            }
        }

        public ICommand SelectCustomerCommand
        {
            get
            {
                if (_selectCustomerCommand == null)
                {
                    _selectCustomerCommand = new Command(p => new PopupWindowViewModel().Show(new SelectCustomerViewModel()));
                }
                return _selectCustomerCommand;
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
        #endregion Bounded Properties

        public AddBuyerViewModel()
        {
            SelectedCustomer = null;
            BuyerToAdd = new Buyer();
        }
        #region Command Functions
        private void AddAnotherBuyer()
        {
            if (SelectedCustomer.Buyers == null) { SelectedCustomer.Buyers = new List<Buyer>(); } // Init the list if null

            BuyerToAdd.Customer = SelectedCustomer; // Set the customer property of the Buyer
            SelectedCustomer.Buyers.Add(BuyerToAdd);
            BuyerToAdd = new Buyer();
        }
        private bool CanAddAnotherBuyer()
        {
            if (SelectedCustomer == null) { return false; }
            return BuyerToAdd.IsFormDataValid();
        }
        private void Save()
        {
            if(BuyerToAdd.IsFormDataValid()) { AddAnotherBuyer(); } // Add the current buyer to the list
            
            using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                conn.Open();
                try
                {
                    BuyersRepository.InsertToDB(conn, SelectedCustomer.Buyers);
                    MessageBox.Show("Successfully inserted");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void Clear()
        {
            SelectedCustomer = null; // Clear the customer
            OnPropertyChanged(nameof(SelectedCustomer));   
            BuyerToAdd = new Buyer(); // Clear the buyer
        }
        #endregion Command Functions

    }
}


using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public Buyer BuyerToAdd { get { return _buyerToAdd; } set { _buyerToAdd = value; } }
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
                    _selectCustomerCommand = new Command(p => new PopupWindowViewModel().Show(new ViewModels.Shared.SelectCustomerViewModel()));
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
            BuyerToAdd = new Buyer();
        }
        #region Command Functions
        private void AddAnotherBuyer()
        {
            if (SelectedCustomer.Buyers == null) { SelectedCustomer.Buyers = new List<Buyer>(); }
            BuyerToAdd.Customer = SelectedCustomer;
            SelectedCustomer.Buyers.Add(BuyerToAdd);
            BuyerToAdd = new Buyer();
            OnPropertyChanged(nameof(BuyerToAdd));
        }
        private bool CanAddAnotherBuyer()
        {
            if (SelectedCustomer == null) { return false; }
            return true;
        }
        private void Save()
        {
            AddAnotherBuyer();
            using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                conn.Open();
                BuyersRepository.InsertToDB(conn, SelectedCustomer.Buyers);
                conn.Close();
            }
        }

        private void Clear()
        {
            SelectedCustomer.Buyers.Clear();
            BuyerToAdd = new Buyer();
        }
        #endregion Command Functions

    }
}

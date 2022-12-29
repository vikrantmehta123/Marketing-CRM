
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Add
{
    public class AddRemarkViewModel : SharedViewModelBase
    {
        #region Fields
        private ICommand _selectCustomerCommand, _saveCommand, _clearCommand;
        private string conn_string = Properties.Settings.Default.conn_string;
        #endregion Fields

        #region Properties
        public Remark RemarkToAdd { get; set; } = new Remark();
        public ICommand SelectCustomerCommand
        {
            get
            {
                if (_selectCustomerCommand == null)
                {
                    _selectCustomerCommand= new Command(p => new PopupWindowViewModel().Show(new SelectCustomerViewModel()));
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
                    _saveCommand = new Command(p =>Save(), p => CanSave());
                }
                return _saveCommand;
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
        #endregion Properties

        public AddRemarkViewModel()
        {
            SelectedCustomer = null; // Clear Selection when the view loads
        }

        #region Methods

        private void Clear()
        {
            RemarkToAdd = new Remark();
            OnPropertyChanged(nameof(RemarkToAdd));
        }
        private void Save()
        {
            RemarkToAdd.Customer = SelectedCustomer;
            using(SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                try
                {
                    RemarksRepository.InsertToDB(conn, RemarkToAdd);
                    MessageBox.Show("Successfully inserted");
                }
                finally
                {
                    conn.Close();
                }
            }   
        }
        private bool CanSave()
        {
            if(SelectedCustomer == null) { return false; }
            return RemarkToAdd.IsDataValid();
        }
        #endregion Methods
    }
}

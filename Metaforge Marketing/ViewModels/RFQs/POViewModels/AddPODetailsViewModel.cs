

using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs.POViewModels
{
    public class AddPODetailsViewModel : POContainer
    {
        #region Fields
        private readonly string conn_string = Properties.Settings.Default.conn_string;
        private ICommand _selectItemCommand, _saveCommand;
        private PO _poToAdd = new PO();
        #endregion Fields

        #region Properties
        public PO POToAdd
        {
            get { return _poToAdd; }
            set { _poToAdd = value; }
        }

        public ICommand SelectItemCommand
        {
            get
            {
                if (_selectItemCommand == null)
                {
                    _selectItemCommand = new Command(p => new PopupWindowViewModel().Show(new SelectPOItemViewModel()));
                }
                return _selectItemCommand;
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand== null)
                {
                    _saveCommand = new Command(p => Save(), p => CanSave());
                }
                return _saveCommand;
            }
        }
        #endregion Properties

        #region Methods

        private void Save()
        {
            using(SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    PORepository.Insert(conn, transaction, _poToAdd, SelectedItem);
                    CostingRepository.UpdateItemStatus(conn, transaction, SelectedItem, Models.Enums.ItemStatusEnum.POReceived);
                    transaction.Commit();
                }
                catch(Exception e1)
                {
                    MessageBox.Show(e1.Message); 
                    try
                    {
                        transaction.Rollback();
                    }
                    catch(Exception e2)
                    {
                        MessageBox.Show(e2.Message);
                    }
                }
                finally 
                { 
                    transaction.Dispose();
                    conn.Close(); 
                }
            }
        }

        private bool CanSave()
        {
            if(SelectedItem == null) { return false; }
            return POToAdd.IsFormDataValid();
        }
        #endregion Methods
    }
}

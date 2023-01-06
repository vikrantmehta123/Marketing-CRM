using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    internal class DetailedRFQViewModel : PopupCloseMarker
    {
        #region Fields
        private ICommand _selectionDoneCommand, _saveCommand;
        private ObservableCollection<Item> _items;
        #endregion Fields

        #region Properties
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
        public override ICommand SelectionDoneCommand
        {
            get
            {
                return _selectionDoneCommand;
            }
        }
        public ObservableCollection<Item> Items
        {
            get
            {
                if (SelectedRFQ != null)
                {
                    _items = new ObservableCollection<Item>(SQLWrapper<Item>.FetchWrapper(Repository.ItemsRepository.FetchItems, SelectedRFQ));
                }
                return _items;
            }
        }
        #endregion Properties


        #region Methods
        public override void ClearSelection() { SelectedItem = null; }

        public override bool IsSelectionDone() { return SelectedItem != null; }

        private void Save()
        {
            foreach (Item item in _items)
            {
                if (item.Status != ItemStatusEnum.Regretted && item.IsRegretted)
                {
                    item.RejectRegret = RejectRegretEnum.Regret;
                    using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        SqlTransaction transaction = conn.BeginTransaction();
                        try
                        {
                            RejectRegretRepository.Insert(conn, transaction, item);
                            CostingRepository.UpdateItemStatus(conn, transaction, item, ItemStatusEnum.Regretted);
                            transaction.Commit();
                            MessageBox.Show("Success");
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            try
                            {
                                transaction.Rollback();
                            }
                            catch(SqlException ex2)
                            {
                                MessageBox.Show(ex2.Message);
                            }
                        }
                        finally
                        {
                            transaction.Dispose();
                            conn.Close();
                        }
                    }
                }
            }
        }
        #endregion Methods
    }
}

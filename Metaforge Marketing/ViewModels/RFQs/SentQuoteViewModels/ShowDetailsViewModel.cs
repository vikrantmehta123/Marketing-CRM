using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs.SentQuoteViewModels
{
    public class ShowDetailsViewModel : SentQuotationsContainer
    {
        #region Fields
        private readonly string conn_string = Properties.Settings.Default.conn_string;
        private ObservableCollection<Item> _items;
        private ICommand _saveCommand;
        #endregion Fields

        public ShowDetailsViewModel()
        {
            _items = GetItems();
            _saveCommand = new Command(p => Save());
        }

        #region Properties
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            private set { _items = value; }
        }
        public ICommand SaveCommand
        {
            get { return _saveCommand;}
        }
        #endregion Properties

        #region Methods
        private ObservableCollection<Item> GetItems()
        {
            ObservableCollection<Item> items;
            using(SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                items = new ObservableCollection<Item>(Repository.ItemsRepository.FetchItems(conn, SelectedRFQ));
                conn.Close();
            }
            return items;
        }

        private void Save()
        {
            foreach (Item item in Items)
            {
                if(item.Status != Models.Enums.ItemStatusEnum.RejectedByCustomer && item.IsRejected)
                {
                    using(SqlConnection conn = new SqlConnection(conn_string))
                    {
                        conn.Open() ;
                        SqlTransaction transaction = conn.BeginTransaction();
                        try
                        {
                            CostingRepository.UpdateItemStatus(conn, transaction, item, Models.Enums.ItemStatusEnum.RejectedByCustomer);
                            item.RejectRegret = Models.Enums.RejectRegretEnum.Reject;
                            RejectRegretRepository.Insert(conn, transaction, item);
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
            }
        }
        #endregion Methods
    }
}

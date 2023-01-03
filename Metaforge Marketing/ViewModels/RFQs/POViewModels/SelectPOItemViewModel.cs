

using Metaforge_Marketing.HelperClasses.Commands;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using Metaforge_Marketing.Models.Enums;
using System.Windows.Input;
using Metaforge_Marketing.HelperClasses;
using System.Windows.Controls;
using System.Windows;

namespace Metaforge_Marketing.ViewModels.RFQs.POViewModels
{
    public class SelectPOItemViewModel : POContainer
    {
        #region Fields
        private readonly string conn_string = Properties.Settings.Default.conn_string;
        private int _count;
        private PaginationCommands<Item> _pagination = new PaginationCommands<Item>();
        private ObservableCollection<Item> _items;
        private ICommand _selectionDoneCommand;
        #endregion Fields

        #region Properties
        public PaginationCommands<Item> Pagination
        {
            get { return _pagination; }
        }
        public ICommand SelectionDoneCommand
        {
            get
            {
                if (_selectionDoneCommand == null)
                {
                    _selectionDoneCommand = new Command(p => Window.GetWindow(((UserControl)p)).DialogResult = true, p => SelectedItem != null);
                }
                return _selectionDoneCommand;
            }
        }
        #endregion Properties

        public SelectPOItemViewModel()
        {
            _items = GetItems();
            _count = GetCount();
            _pagination = new PaginationCommands<Item>(_items, _count, filter);
        }

        #region Methods
        private ObservableCollection<Item> GetItems()
        {
            ObservableCollection<Item> items;
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                items = new ObservableCollection<Item>(ItemsRepository.FetchItems(conn, _pagination.CurrentPage, _pagination.EntriesPerPage, ((int)ItemStatusEnum.QuotationSent)));
                conn.Close();
            }
            return items;
        }
        private int GetCount()
        {
            int count;
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                count = ItemsRepository.CountItems(conn, ((int)ItemStatusEnum.QuotationSent));
                conn.Close();
            }
            return count;
        }


        private bool filter(object o)
        {
            Item item = o as Item;
            if (_pagination != null)
            {
                if (string.IsNullOrEmpty(_pagination.PageSearchText)) { return true; }
                else
                {
                    return (item.ItemName.Contains(_pagination.PageSearchText));
                }
            }
            else { return true; }
        }
        #endregion Methods
    }
}

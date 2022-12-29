using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Reports
{
    public class ItemHistoryReportViewModel : SharedViewModelBase
    {
        private ICommand _selectItemCommand;
        private DataTable _itemHistory;
        
        public ICommand SelectItemCommand
        {
            get
            {
                if(_selectItemCommand == null)
                {
                    _selectItemCommand = new Command(p => new PopupWindowViewModel().Show(new SelectItemViewModel()));
                }
                return _selectItemCommand;  
            }
        }

        public DataTable ItemHistory
        {
            get
            { 
                if(_itemHistory == null && SelectedItem != null)
                {
                    using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _itemHistory = ItemsRepository.FetchItemHistory(conn, SelectedItem);
                        conn.Close();
                    }
                }
                return _itemHistory; 
            }
        }

        public ItemHistoryReportViewModel()
        {
            StaticPropertyChanged += ItemSelectionHandler;
            SelectedItem = null;
        }

        private void ItemSelectionHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedItem))
            {
                OnPropertyChanged(nameof(ItemHistory));
            }
        }

    }
}

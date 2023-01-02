using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.HelperClasses
{
    public class MasterDataPagination : ViewModelBase
    {
        private readonly string conn_string = Properties.Settings.Default.conn_string;
        private int _currentPage, _entriesPerPage = 1, _totalPages;
        private DataTable _masterTable;
        private Action<SqlConnection, DataTable> _updateAction;
        private Func<SqlConnection, int, int, DataTable> _fetchFunc;
        private Func<SqlConnection, int> _countFunc;
        private ICommand _nextPageCommand, _prevPageCommand, _updateCommand;

        public MasterDataPagination(Action<SqlConnection, DataTable> updateAction, Func<SqlConnection, int, int, DataTable> fetchFunc, Func<SqlConnection, int> countFunc)
        {
            _currentPage = 1;
            _updateAction = updateAction;
            _fetchFunc = fetchFunc;
            _countFunc = countFunc;
            _totalPages = ComputeTotalPages(GetCount(), _entriesPerPage);

            _masterTable = GetTable();
        }

        public DataView MasterTable
        {
            get
            {
                return _masterTable.DefaultView;
            }
        }
        public ICommand NextPageCommand
        {
            get
            {
                if (_nextPageCommand == null )
                {
                    _nextPageCommand = new Command(p => Next(), p => CanGoNext());
                }
                return _nextPageCommand;
            }
        }

        public ICommand PrevPageCommand
        {
            get
            {
                if (_prevPageCommand == null)
                {
                    _prevPageCommand = new Command(p => Prev(), p => CanGoPrev());
                }
                return _prevPageCommand;
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                if(_updateCommand == null)
                {
                    _updateCommand = new Command(p => Update());
                }
                return _updateCommand;
            }
        }

        #region Methods

        private void Update()
        {
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                try
                {
                    _updateAction(conn, _masterTable);
                    MessageBox.Show("Success");
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        private void Next()
        {
            _currentPage++;
            _masterTable = GetTable();
            OnPropertyChanged(nameof(MasterTable));
        }
        private void Prev() 
        {
            _currentPage--;
            _masterTable = GetTable();
            OnPropertyChanged(nameof(MasterTable));
        }

        private bool CanGoNext()
        {
            if (_currentPage < _totalPages) { return true; }
            return false;
        }
        private bool CanGoPrev()
        {
            if (_currentPage > 1) { return true; }
            return false;
        }

        private int ComputeTotalPages(int Count, int EntriesPerPage)
        {
            if (Count == 0) { return 0; }
            else if (Count % EntriesPerPage == 0) { return (int)Count / EntriesPerPage; }
            else { return ((int)Count / EntriesPerPage) + 1; }
        }

        private DataTable GetTable()
        {
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                _masterTable = _fetchFunc(conn, (_currentPage - 1) * _entriesPerPage, _entriesPerPage);
                conn.Close();
            }
            return _masterTable;
        }

        private int GetCount()
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                count = _countFunc(conn);
                conn.Close();
            }
            return count;
        }
        #endregion Methods
    }
}

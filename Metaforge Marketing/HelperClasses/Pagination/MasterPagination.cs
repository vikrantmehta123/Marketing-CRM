using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.HelperClasses.Pagination
{
    public class MasterPagination<T>:PaginationBase<T> 
    {
        #region Fields
        private DataTable _masterTable = new DataTable();
        private Action<SqlConnection, DataTable> _updateAction;
        private Func<SqlConnection, int, int, DataTable> _fetchFunc;
        private ICommand _nextPageCommand, _prevPageCommand, _updateCommand;
        #endregion Fields

        #region Properties
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
                if (_nextPageCommand == null)
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
                if (_updateCommand == null)
                {
                    _updateCommand = new Command(p => Update());
                }
                return _updateCommand;
            }
        }
        #endregion Properties

        #region Constructors
        public MasterPagination(Func<SqlConnection, int> countFunction, Action<SqlConnection, DataTable> updateAction, Func<SqlConnection, int, int, DataTable> fetchFunc) : base(countFunction)
        {
            _updateAction = updateAction;
            _fetchFunc = fetchFunc;
            _masterTable = GetTable();
        }
        #endregion Constructors

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
            CurrentPage++;
            _masterTable = GetTable();
            OnPropertyChanged(nameof(MasterTable));
        }
        private void Prev()
        {
            CurrentPage--;
            _masterTable = GetTable();
            OnPropertyChanged(nameof(MasterTable));
        }
        private DataTable GetTable()
        {
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                _masterTable = _fetchFunc(conn, (CurrentPage - 1) * EntriesPerPage, EntriesPerPage);
                conn.Close();
            }
            return _masterTable;
        }

        #endregion Methods 


    }
}

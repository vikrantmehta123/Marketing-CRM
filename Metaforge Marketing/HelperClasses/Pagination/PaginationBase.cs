using Metaforge_Marketing.ViewModels;
using System;
using Microsoft.Data.SqlClient;
using System.Windows.Input;

namespace Metaforge_Marketing.HelperClasses.Pagination
{
    public class PaginationBase<T> : ViewModelBase
    {
        public const int _entriesPerPage = 2;
        protected readonly string conn_string = Properties.Settings.Default.conn_string;

        #region Fields
        private int _currentPage = 1, _totalPages, _offsetIndex;
        private Func<SqlConnection, int> _countFunction;
        #endregion Fields

        #region Constructors
        public PaginationBase(Func<SqlConnection, int> countFunction)
        {
            _countFunction= countFunction;
            _totalPages = ComputeTotalPages(SQLWrapper<T>.CountWrapper(countFunction), _entriesPerPage);
            _offsetIndex = (_currentPage - 1) * _entriesPerPage;
        }

        public PaginationBase(Func<SqlConnection, int, int> countFunction, int status)
        {
            _totalPages = ComputeTotalPages(SQLWrapper<T>.CountWrapper(countFunction, status), _entriesPerPage);
            _offsetIndex = (_currentPage - 1) * _entriesPerPage;
        }
        #endregion Constructors

        #region Properties
        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        public int TotalPages
        {
            get { return _totalPages; }
            set { _totalPages = value; }
        }
        public int EntriesPerPage
        {
            get { return _entriesPerPage; }
        }
        protected int OffsetIndex
        {
            get { return _offsetIndex; }
            set { _offsetIndex = value; }
        }
        #endregion Properties

        #region Command Functions
        protected bool CanGoPrev()
        {
            if (CurrentPage > 1) { return true; }
            return false;
        }
        protected bool CanGoNext()
        {
            if (CurrentPage < TotalPages) { return true; }
            return false;
        }
        protected bool CanGoLast()
        {
            if (CurrentPage == TotalPages) { return false; }
            return true;
        }
        protected bool CanGoFirst()
        {
            if (CurrentPage == 1) { return false; }
            return true;
        }
        protected int ComputeTotalPages(int Count, int EntriesPerPage)
        {
            if (Count == 0) { return 0; }
            else if (Count % EntriesPerPage == 0) { return (int)Count / EntriesPerPage; }
            else { return ((int)Count / EntriesPerPage) + 1; }
        }
        #endregion Command Functions
    }
}

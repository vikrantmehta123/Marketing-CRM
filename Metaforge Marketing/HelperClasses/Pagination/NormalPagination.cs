using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Metaforge_Marketing.HelperClasses.Pagination
{
    public class NormalPagination<T> : PaginationBase<T>
    {
        #region Fields
        private string _pageSearchText;
        private Func<SqlConnection, int, int, IEnumerable<T>> _fetchFunction;
        private Predicate<object> _filterFunction;
        private Func<SqlConnection, string, IEnumerable<T>> _searchFunction;
        private ICommand _nextPageCommand, _prevPageCommand, _lastPageCommand, _firstPageCommand, _searchDatabaseCommand;
        private ObservableCollection<T> _collection;
        #endregion Fields

        #region Commands
        public ICommand NextPageCommand
        {
            get
            {
                if (_nextPageCommand == null)
                {
                    _nextPageCommand = new Command(p => NextPage(), p => CanGoNext());
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
                    _prevPageCommand = new Command(p => PrevPage(), p => CanGoPrev());
                }
                return _prevPageCommand;
            }
        }
        public ICommand LastPageCommand
        {
            get
            {
                if (_lastPageCommand == null)
                {
                    _lastPageCommand = new Command(p => LastPage(), p => CanGoLast());
                }
                return _lastPageCommand;
            }
        }
        public ICommand FirstPageCommand
        {
            get
            {
                if (_firstPageCommand == null)
                {
                    _firstPageCommand = new Command(p => FirstPage(), p => CanGoFirst());
                }
                return _firstPageCommand;
            }
        }

        public ICommand SearchDatabaseCommand
        {
            get
            {
                if (_searchDatabaseCommand == null)
                {
                    _searchDatabaseCommand = new Command(p => {
                        _collection = new ObservableCollection<T>(SQLWrapper<T>.SearchWrapper(_searchFunction, DBSearchText));
                        OnPropertyChanged(nameof(Collection));
                        CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_collection);
                        view.Filter = _filterFunction;
                    });
                }
                return _searchDatabaseCommand;
            }
        }
        #endregion Commands

        #region Properties
        public string DBSearchText { get; set; }
        public ObservableCollection<T> Collection
        {
            get { return _collection; }
            private set
            {
                _collection = value;
            }
        }

        public string PageSearchText
        {
            get { return _pageSearchText; }
            set
            {
                _pageSearchText = value;
                CollectionViewSource.GetDefaultView(Collection).Refresh();
            }
        }

        #endregion Properties

        #region Constructors
        public NormalPagination(Func<SqlConnection, int, int, IEnumerable<T>> fetchFunction, Func<SqlConnection, int> countFunction, Predicate<object> filterFunction) : base(countFunction)
        {
            _fetchFunction = fetchFunction;
            _filterFunction= filterFunction;
            _collection = new ObservableCollection<T>(SQLWrapper<T>.FetchWrapper(fetchFunction, OffsetIndex, EntriesPerPage));
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_collection);
            view.Filter = _filterFunction;
        }

        public NormalPagination(Func<SqlConnection, int, int, IEnumerable<T>> fetchFunction, Func<SqlConnection, int> countFunction,
            Func<SqlConnection, string, IEnumerable<T>> searchFunction, Predicate<object> filterFunction) : this(fetchFunction, countFunction, filterFunction)
        {
            _searchFunction = searchFunction;
        }
        #endregion Constructors

        #region Command Functions
        private void NextPage()
        {
            CurrentPage++;
            Collection = new ObservableCollection<T>(SQLWrapper<T>.FetchWrapper(_fetchFunction, (this.CurrentPage - 1) * EntriesPerPage, EntriesPerPage));
            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(Collection));
        }
        private void PrevPage()
        {
            CurrentPage--;
            Collection = new ObservableCollection<T>(SQLWrapper<T>.FetchWrapper(_fetchFunction, (this.CurrentPage - 1) * EntriesPerPage, EntriesPerPage));
            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(Collection));
        }
        private void FirstPage()
        {
            CurrentPage = 1;
            Collection = new ObservableCollection<T>(SQLWrapper<T>.FetchWrapper(_fetchFunction, (this.CurrentPage - 1) * EntriesPerPage, EntriesPerPage));
            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(Collection));
        }
        private void LastPage()
        {
            CurrentPage = TotalPages;
            Collection = new ObservableCollection<T>(SQLWrapper<T>.FetchWrapper(_fetchFunction, (this.CurrentPage - 1) * EntriesPerPage, EntriesPerPage));
            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(Collection));
        }

        #endregion Command Functions
    }
}

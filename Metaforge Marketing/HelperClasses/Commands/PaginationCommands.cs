using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Metaforge_Marketing.HelperClasses.Commands
{
    public class PaginationCommands<T> : INotifyPropertyChanged
    {
        #region INPC

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INPC

        #region Fields
        private int _currentPage, _totalPages, _offsetIndex,_entriesPerPage = 2;
        private string conn_string = Metaforge_Marketing.Properties.Settings.Default.conn_string, _pageSearchText;
        private Func<SqlConnection, int, int, IEnumerable<T>> _fetchFunction;
        private Func<SqlConnection, int> _countFunction;
        private Func<SqlConnection, string, IEnumerable<T>> _searchFunction;
        private Predicate<object> _filterFunction;
        private ObservableCollection<T> _collection;
        private ICommand _searchDatabaseCommand, _nextPageCommand, _lastPageCommand, _firstPageCommand, _prevPageCommand;
        #endregion Fields

        #region Properties
        public int EntriesPerPage { get { return _entriesPerPage; } }
        public int CurrentPage { get { return _currentPage; } private set { _currentPage = value; } }
        public int TotalPages { get { return _totalPages; } private set { _totalPages = value; } }
        public string DBSearchText { get; set; }
        public string PageSearchText
        {
            get { return _pageSearchText; }
            set
            {
                _pageSearchText = value;
                CollectionViewSource.GetDefaultView(Collection).Refresh();
            }
        }
        public ObservableCollection<T> Collection
        {
            get { return _collection; }
            private set
            {
                _collection = value;
            }
        }
        #endregion Properties

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
                if (_prevPageCommand== null)
                {
                    _prevPageCommand = new Command(p => PrevPage(), p=> CanGoPrev());
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
                    _searchDatabaseCommand = new Command( p => { 
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

        #region Constructors
        public PaginationCommands()
        {
            _currentPage = 1;
            _offsetIndex = (_currentPage - 1) * _entriesPerPage;
        }
        public PaginationCommands(Func<SqlConnection, int, int, IEnumerable<T>> fetchFunction, Func<SqlConnection, int> countFunction,
            Func<SqlConnection, string, IEnumerable<T>> searchFunction, Predicate<object> filterFunction) :
            this()
        {
            _fetchFunction = fetchFunction;
            _searchFunction = searchFunction;
            _countFunction = countFunction;
            _filterFunction = filterFunction;

            _collection = new ObservableCollection<T>(SQLWrapper<T>.FetchWrapper(fetchFunction, _offsetIndex, _entriesPerPage));
            CollectionView view = (CollectionView) CollectionViewSource.GetDefaultView(_collection);
            view.Filter = _filterFunction;
        }

        public PaginationCommands(IEnumerable<T> collection, int count, Predicate<object> filterFunction) : this()
        {
            _filterFunction = filterFunction;
            CurrentPage = 1;
            _collection = new ObservableCollection<T>(collection);
            TotalPages = ComputeTotalPages(count, EntriesPerPage); 
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(_collection);
            view.Filter = _filterFunction;
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
        private bool CanGoPrev()
        {
            if (CurrentPage > 1) { return true; }
            return false;
        }
        private bool CanGoNext()
        {
            if (CurrentPage < TotalPages) { return true; }
            return false;
        }
        private bool CanGoLast()
        {
            if (CurrentPage == TotalPages) { return false; }
            return true;
        }
        private bool CanGoFirst()
        {
            if (CurrentPage == 1) { return false; }
            return true;
        }
        #endregion Command Functions

        #region Methods
        private int ComputeTotalPages(int Count, int EntriesPerPage)
        {
            if (Count % EntriesPerPage == 0) { return (int)Count / EntriesPerPage; }
            else { return ((int)Count / EntriesPerPage) + 1; }
        }
        #endregion Methods
    }
}

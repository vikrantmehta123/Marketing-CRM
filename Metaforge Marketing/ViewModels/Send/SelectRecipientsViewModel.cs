using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.HelperClasses.Commands;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Send
{
    public class SelectRecipientsViewModel : GeneralMailContainer
    {
        private int _currentPage;
        private ObservableCollection<Buyer> _buyersList= new ObservableCollection<Buyer>();
        private ICommand _nextPageCommand, _prevPageCommand, _lastPageCommand, _firstPageCommand;

        #region Properties
        public ObservableCollection<Buyer> BuyersList
        {
            get
            {
                return new ObservableCollection<Buyer>(VisitedRecipients.Skip((_currentPage - 1) * _entriesPerPage).Take(_entriesPerPage));
            }
            private set
            {
                if (value != _buyersList)
                {
                    _buyersList = value;
                    OnPropertyChanged(nameof(BuyersList));
                }
            }
        }

        public ICommand DoneCommand => new Command(p =>
                                                    {
                                                        Done();
                                                        Window.GetWindow((UserControl)p).Close();
                                                    });

        #endregion Properties

        #region Pagination Commands
        public ICommand PrevPageCommand
        {
            get 
            { 
                if(_prevPageCommand == null)
                {
                    _prevPageCommand = new Command(p => 
                    { 
                        _currentPage--; 
                        OnPropertyChanged(nameof(BuyersList));
                    }, p => CanGoPrev());
                }
                return _prevPageCommand; 
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

        public ICommand LastPageCommand
        {
            get
            {
                if(_lastPageCommand == null)
                {
                    _lastPageCommand = new Command(p =>
                    {
                        _currentPage = TotalPages;
                    });
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
                    _firstPageCommand = new Command(p => _currentPage = 1);
                }
                return _firstPageCommand;
            }
        }
        #endregion Pagination Commands
        public SelectRecipientsViewModel()
        {
            _currentPage = 1;
        }


        #region Methods
        private void Next()
        {
            _currentPage++;
            if (VisitedRecipients.Count <= (_currentPage - 1) * _entriesPerPage)
            {
                VisitedRecipients.AddRange(SQLWrapper<Buyer>.FetchWrapper(BuyersRepository.FetchBuyers, (_currentPage - 1) * _entriesPerPage, _entriesPerPage));  
            }
            OnPropertyChanged(nameof(BuyersList));
        }
        private bool CanGoPrev()
        {
            if (_currentPage > 1) { return true; }
            return false;   
        }
        private bool CanGoNext()
        {
            if (_currentPage < TotalPages) { return true; }
            return false;
        }

        private void Done()
        {
            foreach (Buyer buyer in VisitedRecipients)
            {
                if (buyer.IsChecked)
                {
                    Recipients.Add(buyer);
                }
                else if (!buyer.IsChecked && Recipients.Contains(buyer))
                {
                    Recipients.Remove(buyer);
                }
            }
        }
        #endregion Methods
    }
}

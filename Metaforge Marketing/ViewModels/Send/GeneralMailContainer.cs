
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using System.Collections.Generic;

namespace Metaforge_Marketing.ViewModels.Send
{
    public abstract class GeneralMailContainer : ViewModelBase
    {
        private static List<Buyer> _visitedRecipients;
        private static int _totalPages;
        public static Email Email { get; set; } = new Email();

        protected static int _entriesPerPage = 4;
        protected static int TotalPages
        {
            get
            {
                if (_totalPages == 0)
                {
                    int count = SQLWrapper<Buyer>.CountWrapper(BuyersRepository.CountBuyers);
                    _totalPages = ComputeTotalPages(count, _entriesPerPage);
                }
                return _totalPages;
            }
        }

        public static List<Buyer> VisitedRecipients
        {
            get
            {
                if (_visitedRecipients == null)
                {
                    _visitedRecipients = new List<Buyer>(SQLWrapper<Buyer>.FetchWrapper(BuyersRepository.FetchBuyers, offsetIndex: 0, _entriesPerPage));
                }
                return _visitedRecipients;
            }
            protected set
            {
                if (_visitedRecipients != value)
                {
                    _visitedRecipients = value;
                }
            }
        }

        #region Methods
        private static int ComputeTotalPages(int Count, int EntriesPerPage)
        {
            if (Count % EntriesPerPage == 0) { return (int)Count / EntriesPerPage; }
            else { return ((int)Count / EntriesPerPage) + 1; }
        }
        #endregion Methods
    }
}

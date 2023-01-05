

using System;
using System.Collections.Generic;

namespace Metaforge_Marketing.Models
{
    public class RFQ
    {
        #region Fields
        private DateTime _enquiryDate;
        private int _id;
        private string _projectName, _referredBy;
        private Customer _customer;
        private Buyer _buyer;
        private List<Item> _items;
        private Admin _rfqBroughtBy;
        #endregion Fields

        #region Properties
        public DateTime EnquiryDate { get { return _enquiryDate; } set { _enquiryDate = value; } }
        public int Id { get { return _id;} set { _id = value; } }
        public string ProjectName { get { return _projectName;} set { _projectName = value; } }
        public string ReferredBy { get { return _referredBy;} set { _referredBy = value; } }
        public Customer Customer { get { return _customer;} set { _customer = value; } }
        public Buyer Buyer { get { return _buyer; } set { _buyer = value; } }
        public Admin RFQBroughtBy { get { return _rfqBroughtBy; } set { _rfqBroughtBy = value; } }
        public List<Item> Items { get { return _items; } set { _items = value; } }
        #endregion Properties

        public RFQ()
        {
            _items = new List<Item>();
            _enquiryDate = DateTime.Today.Date;
        }

        #region Methods
        // Summary:
        //      Basic validation
        public bool IsFormDataValid()
        {
            if (String.IsNullOrEmpty(_projectName)) { return false; }
            if(EnquiryDate > DateTime.Today.Date) { return false;}
            if (RFQBroughtBy == null) { return false; }
            return true;
        }
        #endregion Methods
    }
}

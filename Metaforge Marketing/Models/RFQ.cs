

using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Metaforge_Marketing.Models
{
    public class RFQ
    {
        #region Fields
        private DateTime _enquiryDate;
        private int _id;
        private string _projectName, _referredBy;
        private Customer _customer;
        private List<Item> _items;
        #endregion Fields

        #region Properties
        public DateTime EnquiryDate { get { return _enquiryDate; } set { _enquiryDate = value; } }
        public int Id { get { return _id;} set { _id = value; } }
        public string ProjectName { get { return _projectName;} set { _projectName = value; } }
        public string ReferredBy { get { return _referredBy;} set { _referredBy = value; } }
        public Customer Customer { get { return _customer;} set { _customer = value; } }

        public List<Item> Items { get { return _items; } set { _items = value; } }
        #endregion Properties

        public RFQ()
        {
            _items = new List<Item>();
            _enquiryDate = DateTime.Today.Date;
        }
    }
}

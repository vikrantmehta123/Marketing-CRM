

using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaforge_Marketing.Models
{
    public class Customer
    {
        public static IEnumerable<CustomerCategoryEnum> Categories { get; } = Enum.GetValues(typeof(CustomerCategoryEnum)).Cast<CustomerCategoryEnum>();

        #region Fields
        private int _id, _buyerCount = -1;
        private string _customerName, _city, _address, _pincode, _referredBy;
        private List<Buyer> _buyers;
        private CustomerCategoryEnum _category;
        #endregion Fields

        #region Proeprties
        public int Id { get { return _id; } set { _id = value; } }
        public int BuyerCount
        {
            get
            {
                if (_buyerCount == -1)
                {
                    _buyerCount = SQLWrapper<Customer>.CountWrapper(Repository.CustomersRepository.CountBuyers, this);
                }
                return _buyerCount;
            }
        }
        public string CustomerName { get { return _customerName;} set { _customerName = value; } }
        public string City { get { return _city;} set { _city = value; } }
        public string Address { get { return _address;} set { _address = value; } }
        public string Pincode { get { return _pincode;} set { _pincode = value; } }
        public string ReferredBy { get { return _referredBy; } set { _referredBy = value; } }  
        public CustomerCategoryEnum Category { get { return _category; } set { _category = value; } }
        public List<Buyer> Buyers { get { return _buyers; } set { _buyers = value; } }
        #endregion Properties
    }
}

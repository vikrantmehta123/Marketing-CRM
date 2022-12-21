

using Metaforge_Marketing.HelperClasses;
using System.Collections.Generic;

namespace Metaforge_Marketing.Models
{
    public class Customer
    {
        private int _id, _buyerCount = -1;
        private string _customerName, _city, _address, _pincode;
        private List<Buyer> _buyers;

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

        public List<Buyer> Buyers { get { return _buyers; } set { _buyers = value; } }
        #endregion Properties
    }
}

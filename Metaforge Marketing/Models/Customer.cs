

namespace Metaforge_Marketing.Models
{
    public class Customer
    {
        private int _id;
        private string _customerName, _city, _address, _pincode;

        #region Proeprties
        public int Id { get { return _id; } set { _id = value; } }
        public string CustomerName { get { return _customerName;} set { _customerName = value; } }
        public string City { get { return _city;} set { _city = value; } }
        public string Address { get { return _address;} set { _address = value; } }
        public string Pincode { get { return _pincode;} set { _pincode = value; } }

        #endregion Properties
    }
}

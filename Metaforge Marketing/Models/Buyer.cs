

namespace Metaforge_Marketing.Models
{
    public class Buyer
    {
        #region Fields
        private string _name, _email, _phone;
        private int _id;
        private Customer _customer;
        #endregion Fields

        #region Properties
        public string Name { get { return _name;} set { _name = value; } }
        public string Email { get { return _email;} set { _email = value; } }
        public int Id { get { return _id;} set { _id = value; } }
        public string Phone { get { return _phone;} set { _phone = value; } }
        public Customer Customer { get { return _customer;} set { _customer = value; } }
        #endregion Properties
    }
}

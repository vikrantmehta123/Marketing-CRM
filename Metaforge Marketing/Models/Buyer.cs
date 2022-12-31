

using System;

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
        public bool IsChecked { get; set; } // For when selecting Recipients
        public string Phone { get { return _phone;} set { _phone = value; } }
        public Customer Customer { get { return _customer;} set { _customer = value; } }
        #endregion Properties

        #region Methods
        // Summary:
        //      Performs basic validation on the "Add Buyer" form
        public bool IsFormDataValid()
        {
            if (String.IsNullOrEmpty(Name)) { return false; }
            if (String.IsNullOrEmpty(Email)) { return false; }
            return true;
        }
        #endregion Methods

    }
}

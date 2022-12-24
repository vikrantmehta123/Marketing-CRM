
using System;

namespace Metaforge_Marketing.Models
{
    public class Remark
    {

        public Customer Customer { get; set; }
        public DateTime EventDate { get; set; }
        public int Id { get; set; }
        public string Note { get; set; }


        #region Methods
        public bool IsDataValid()
        {
            if (Customer == null) { return false; }
            if (EventDate > DateTime.Today) { return false; }
            if (String.IsNullOrEmpty(Note)) { return false; }
            return true;
        }
        #endregion Methods

    }
}

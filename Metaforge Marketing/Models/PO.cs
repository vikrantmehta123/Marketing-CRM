

using System;

namespace Metaforge_Marketing.Models
{
    public class PO
    {
        #region Fields
        private int _id, _qty;
		private string _number, _note;
		private DateTime _date = DateTime.Today;
        #endregion Fields

        #region Properties
		public DateTime Date
		{
			get { return _date; }
			set { _date = value; }
		}

        public int Qty
		{
			get { return _qty; }
			set { _qty = value; }
		}

		public string Note
		{
			get { return _note; }
			set { _note = value; }
		}

		public string Number
		{
			get { return _number; }
			set { _number = value; }
		}

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}
		#endregion Properties

		#region Methods
		public bool IsFormDataValid()
		{
			if (Qty == 0) { return false; }
			if(String.IsNullOrEmpty(Number)) { return false; }
			if(Date > DateTime.Today) { return false; }
			return true;
		}
        #endregion Methods
    }
}

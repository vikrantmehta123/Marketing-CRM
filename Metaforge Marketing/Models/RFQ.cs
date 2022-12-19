

using System;

namespace Metaforge_Marketing.Models
{
    public class RFQ
    {
        #region Fields
        private DateTime _enquiryDate;
        private int _id;
        private string _projectName, _referredBy;
        #endregion Fields

        #region Properties
        public DateTime EnquiryDate { get { return _enquiryDate; } set { _enquiryDate = value; } }
        public int Id { get { return _id;} set { _id = value; } }
        public string ProjectName { get { return _projectName;} set { _projectName = value; } }
        public string ReferredBy { get { return _referredBy;} set { _referredBy = value; } }
        #endregion Properties
    }
}

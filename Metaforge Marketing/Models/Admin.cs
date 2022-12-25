
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.ComponentModel;
using System.Windows;

namespace Metaforge_Marketing.Models
{
    public class Admin : SharedModelsBase
    {
        #region Fields
        private int _id, _rfqCount = -1, _preparedCostingsCount = -1, _convertedQuotationsCount = -1;
        private float _avgResponseTime = -1, _conversionRate = -1;
        private string _name;
        private string conn_string = Properties.Settings.Default.conn_string;
        #endregion Fields

        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        #endregion Properties

        #region Properties For Performance Review
        public int RFQCount
        {
            get { return _rfqCount; }
            set { _rfqCount = value; }
        }
        public int PreparedCostingsCount 
        { 
            get { return _preparedCostingsCount; }
            set { _preparedCostingsCount = value; }
        }
        public int ConvertedQuotationsCount 
        { 
            get { return _convertedQuotationsCount; }
            set { _convertedQuotationsCount = value; }
        }
        public float ConversionRate
        {
            get
            {
                if (_preparedCostingsCount > 0)
                {
                    _conversionRate = _convertedQuotationsCount / _preparedCostingsCount * 100;
                }
                return _conversionRate;
            }
        }
        public float AvgResponseTime 
        { 
            get { return _avgResponseTime; }
            set { _avgResponseTime = value; }
        }


        #endregion Properties For Performance Review

        #region Methods
        public override string ToString() { return Name; }
        #endregion Methods
    }
}

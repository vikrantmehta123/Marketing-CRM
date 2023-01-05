
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.ComponentModel;
using System.Windows;

namespace Metaforge_Marketing.Models
{
    public class Admin : ModelsBase
    {
        #region Fields
        private int _id, _rfqCount = -1, _preparedCostingsCount = -1, _convertedQuotationsCount = -1;
        private float _avgResponseTime = -1, _conversionRate = -1, _totalBusinessBrought;
        private string _name;
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
        public float TotalBusinessBrought
        {
            get { return _totalBusinessBrought; }
            set { _totalBusinessBrought = value; }
        }
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

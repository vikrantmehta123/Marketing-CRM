
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
            get 
            { 
                using (SqlConnection conn = new SqlConnection(conn_string))
                {
                    conn.Open();
                    _rfqCount = AdminsRepository.CountRFQs(conn, this, StartDate, EndDate);
                    conn.Close();
                }
                return _rfqCount; 
            }
        }
        public int PreparedCostingsCount
        {
            get
            {
                using (SqlConnection conn = new SqlConnection(conn_string))
                {
                    conn.Open();
                    _preparedCostingsCount = AdminsRepository.CountPreparedQuotations(conn, this, StartDate, EndDate);
                    conn.Close();
                }
                return _preparedCostingsCount;
            }

        }
        public int ConvertedQuotationsCount
        {
            get
            {
                using (SqlConnection conn = new SqlConnection(conn_string))
                {
                    conn.Open();
                    _convertedQuotationsCount = AdminsRepository.CountConvertedQuotations(conn, this, StartDate, EndDate);
                    conn.Close();
                }
                return _convertedQuotationsCount;
            }
        }
        public float ConversionRate
        {
            get
            {
                _conversionRate = _convertedQuotationsCount / _preparedCostingsCount* 100;
                return _conversionRate;
            }
        }
        public float AvgResponseTime
        {
            get
            {
                return _avgResponseTime;
            }
        }

        #endregion Properties For Performance Review

        public Admin()
        {
            StaticPropertyChanged += DateChangedHandler;
        }

        #region Methods
        private void DateChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName== nameof(StartDate) || e.PropertyName == nameof(EndDate))
            {
                OnPropertyChanged(nameof(RFQCount));
                OnPropertyChanged(nameof(ConvertedQuotationsCount));
                OnPropertyChanged(nameof(ConversionRate));
                OnPropertyChanged(nameof(AvgResponseTime));
                OnPropertyChanged(nameof(PreparedCostingsCount));
            }
        }
        public override string ToString() { return Name; }
        #endregion Methods
    }
}

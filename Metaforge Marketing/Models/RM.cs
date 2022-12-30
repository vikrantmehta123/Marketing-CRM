using Metaforge_Marketing.Models.Enums;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Metaforge_Marketing.Models
{
    public class RM 
    {
        public IEnumerable<RMCategoryEnum> RMCategories { get; } = Enum.GetValues(typeof(RMCategoryEnum)).Cast<RMCategoryEnum>();

        #region Fields
        private int _id;
        private string _grade;
        private RMCategoryEnum _category;
        private float _currentRate;
        private List<RM> _rmMasterData = new List<RM>();
        #endregion Fields

        #region Properties
        public int Id { get { return _id; } set { _id = value; } }
        public RMCategoryEnum Category { get { return _category; } set { _category = value; } }
        public string Grade { get { return _grade; } set { _grade = value; } }
        public float CurrentRate 
        { 
            get { return _currentRate;} 
            set { 
                if(_currentRate != value )
                {
                    _currentRate = value;
                }
            }
        }

        public List<RM> RMMaster { 
            get
            {
                using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                {
                    conn.Open();
                    _rmMasterData = RMRepository.FetchRMs(conn).ToList();
                    conn.Close();
                }
                return _rmMasterData;
            }
        }
        #endregion Properties

        #region Methods

        public bool IsFormDataValid()
        {
            if(String.IsNullOrEmpty(Grade)) { return false; }
            if (CurrentRate <= 0) { return false; }
            return true;
        }
        public override string ToString()
        {
            if (((int)Category) == 0) { return Grade; }
            return Grade + " " + Category.ToString();
        }

        #endregion Methods
    }
}

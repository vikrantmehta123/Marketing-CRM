

using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Metaforge_Marketing.Models
{
    public class Operation : ModelsBase
    {
        #region Static Fields
        private static List<Operation> _allOperations;
        public static List<Operation> AllOperations
        {
            get
            {
                if (_allOperations == null)
                {
                    using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _allOperations = new List<Operation>(Repository.OperationsRepository.FetchOperations(conn));
                        conn.Close();
                    }
                }
                return _allOperations;
            }
        }
        #endregion Static Fields

        #region Fields
        private string _operationName;
        private int _id, _stepNo;
        private float _costPerPiece;
        private bool _isOutsourced;
        #endregion Fields

        #region Properties
        public int Id { get { return _id;} set { _id = value;} }
        public int StepNo { get { return _stepNo; } set { _stepNo = value;} }
        public string OperationName { get { return _operationName; } set { _operationName = value; } }
        public float CostPerPiece { 
            get { 
                return _costPerPiece; 
            } 
            set { 
                _costPerPiece = value;
            }
        }
        public bool IsOutsourced { get { return _isOutsourced; } set { _isOutsourced = value;} }
        #endregion Properties

        #region Methods
        public override string ToString()
        {
            return OperationName;
        }

        #endregion Methods
    }
}

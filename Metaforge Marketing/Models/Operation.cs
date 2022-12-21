

using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace Metaforge_Marketing.Models
{
    public class Operation
    {
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

        #region Fields
        private string _operationName;
        private int _id;
        private float _efficiency, _mchr, _cycleTime, _costPerPiece;
        private bool _isOutsourced;
        #endregion Fields

        #region Properties
        public int Id { get { return _id;} set { _id = value;} }
        public string OperationName { get { return _operationName; } set { _operationName = value; } }
        public float Efficiency { get { return _efficiency;} set { _efficiency = value; } }
        public float MCHr { get { return _mchr; } set { _mchr= value; } }
        public float CostPerPiece { get { return _costPerPiece; } set { _costPerPiece = value; } }
        public float CycleTime { get { return _cycleTime; } set { _cycleTime = value; } }

        public bool IsOutsourced { get { return _isOutsourced; } set { _isOutsourced = value;} }
        #endregion Properties


        #region Methods

        public float ComputeCostPerPiece()
        {
            int SecondsInAnHour = 3600;
            return MCHr / ((SecondsInAnHour / CycleTime) * (Efficiency / 100));
        }
        public override string ToString()
        {
            return OperationName;
        }
        #endregion Methods
    }
}



namespace Metaforge_Marketing.Models
{
    public class Operation
    {
        #region Fields
        private string _operationName;
        private int _id;
        private float _efficiency, _mchr, cycleTime, _costPerPiece;
        #endregion Fields

        #region Properties
        public int Id { get { return _id;} set { _id = value;} }
        public string OperationName { get { return _operationName; } set { _operationName = value; } }
        public float Efficiency { get { return _efficiency;} set { _efficiency = value; } }
        public float MCHr { get { return _mchr; } set { _mchr= value; } }
        public float CostPerPiece { get { return _costPerPiece; } set { _costPerPiece = value; } }
        #endregion Properties


        #region Methods
        #endregion Methods
    }
}

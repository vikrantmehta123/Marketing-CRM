
using System.Collections.Generic;
using System.Linq;

namespace Metaforge_Marketing.Models
{
    public class ConversionCosting
    {
        #region Fields
        private bool _isConvCostingPresent, _isConvCostingDetailsPresent;
        private float _totalCostPerPiece;
        private List<Operation> _operations;
        private Item _item;
        #endregion Fields

        #region Properties
        public List<Operation> Operations { get { return _operations;} set { _operations = value; } }

        public float TotalCostPerPiece
        {
            get 
            {
                _totalCostPerPiece = _operations.Sum(op => op.CostPerPiece);
                return _totalCostPerPiece;
            }
        }

        public bool IsConvCostingPresent
        {
            get { return _isConvCostingPresent; }
            set { _isConvCostingPresent = value;}
        }
        public bool IsConvCostingDetailsPresent
        {
            get { return _isConvCostingDetailsPresent; }
            set { _isConvCostingDetailsPresent = value; }
        }
        #endregion Properties


    }
}

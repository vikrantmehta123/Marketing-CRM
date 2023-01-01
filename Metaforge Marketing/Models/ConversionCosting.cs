using System.Collections.ObjectModel;
using System.Linq;

namespace Metaforge_Marketing.Models
{
    public class ConversionCosting :  ModelsBase
    {
        #region Fields
        private bool _isConvCostingPresent;
        private float _totalCostPerPiece;
        private ObservableCollection<Operation> _operations;
        #endregion Fields

        #region Properties
        public ObservableCollection<Operation> Operations { get { return _operations;} set { _operations = value; } }

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
        #endregion Properties


    }
}

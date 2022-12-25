


namespace Metaforge_Marketing.Models
{
    public class RMCosting : ModelsBase
    {
        #region Fields
        private int _id;
        private string _rmAsPerDrawing;
        private float _rmRate, _scrapRate, _scrapRecovery, _cuttingAllowance, _itemLength, _costPerPiece;
        private bool _isRMManuallySet, _isRMCostingPresent, _isRMCostingDetailsPresent;
        private RM _rmConsidered = new RM();
        private Admin _costingPreparedBy;
        #endregion Fields

        #region Properties
        public int Id { get { return _id; } set { _id = value; } }
        public string RMAsPerDrawing { get { return _rmAsPerDrawing; } set { _rmAsPerDrawing = value;} }
        public Admin CostingPreparedBy
        {
            get { return _costingPreparedBy; }
            set { _costingPreparedBy = value; }
        }
        public RM RMConsidered { 
            get { return _rmConsidered;} 
            set { 
                _rmConsidered = value;
                _isRMManuallySet = false;
                OnPropertyChanged(nameof(RMRate));
            }
        }
        public float RMRate 
        { 
            get 
            { 
                if (RMConsidered != null && !_isRMManuallySet) { _rmRate = RMConsidered.CurrentRate; }
                return _rmRate; 
            } 
            set 
            { 
                _rmRate = value;
                _isRMManuallySet = true;
            } 
        }

        public float ScrapRate { get { return _scrapRate; } set { _scrapRate= value; } }
        public float CuttingAllowance { get { return _cuttingAllowance; } set { _cuttingAllowance = value; } }
        public float ItemLength { get { return _itemLength; } set { _itemLength = value; } }
        public float ScrapRecovery { get { return _scrapRecovery; } set { _scrapRecovery= value; } }

        public float CostPerPiece
        {
            get 
            {   if (_scrapRate != 0 && _scrapRecovery != 0 && _rmRate != 0 && _scrapRecovery != 0) 
                {
                    // TODO: Implement a formula to compute cost per piece of raw material
                }
                return _costPerPiece; 
            }
            set { _costPerPiece = value; }
        }

        #endregion Properties


        #region Methods
        public bool IsDataValid()
        {
            if (RMRate == 0) { return false; }
            if (RMConsidered == null) { return false; }
            return true;
        }


        #endregion Methods
    }
}

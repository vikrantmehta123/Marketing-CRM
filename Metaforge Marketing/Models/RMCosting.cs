


namespace Metaforge_Marketing.Models
{
    public class RMCosting : ModelsBase
    {
        #region Fields
        private int _id;
        private string _rmAsPerDrawing;
        private float _rmRate, _costPerPiece;
        private bool _isRMCostingPresent;
        private RM _rmConsidered = new RM();
        private Admin _costingPreparedBy;
        #endregion Fields

        #region Properties
        public int Id { get { return _id; } set { _id = value; } }
        public string RMAsPerDrawing 
        { 
            get { return _rmAsPerDrawing; } 
            set { 
                _rmAsPerDrawing = value;
                OnPropertyChanged(nameof(RMAsPerDrawing));
            }
        }
        public bool IsRMCostingPresent { get { return _isRMCostingPresent; } set { _isRMCostingPresent= value; } }
        public Admin CostingPreparedBy
        {
            get { return _costingPreparedBy; }
            set { _costingPreparedBy = value; }
        }
        public RM RMConsidered { 
            get { return _rmConsidered;} 
            set { 
                if (_rmConsidered != value)
                {
                    _rmConsidered = value;
                    OnPropertyChanged(nameof(RMConsidered));
                }
            }
        }
        public float RMRate 
        { 
            get { return _rmRate; } 
            set 
            { 
                if(_rmRate != value)
                {
                    _rmRate = value;
                    OnPropertyChanged(nameof(RMRate));  
                }
            } 
        }
        public float CostPerPiece
        {
            get { return _costPerPiece; }
            set { _costPerPiece = value; }
        }

        #endregion Properties


        public RMCosting()
        {
            RMConsidered.PropertyChanged += RMConsidered_PropertyChanged;
        }

        #region Methods
        public bool IsDataValid()
        {
            if (RMRate == 0) { return false; }
            if (RMConsidered == null) { return false; }
            return true;
        }

        public float ComputeRMCost(Item item)
        {
            return (float)(item.GrossWeight * RMRate / 1000);
        }

        private void RMConsidered_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RMConsidered));
        }
        #endregion Methods
    }
}

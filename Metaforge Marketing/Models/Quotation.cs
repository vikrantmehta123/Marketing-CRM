using Metaforge_Marketing.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaforge_Marketing.Models
{
    public class Quotation : ModelsBase
    {
        public IEnumerable<CostingCategoryEnum> CostingCategories { get; } = Enum.GetValues(typeof(CostingCategoryEnum)).Cast<CostingCategoryEnum>();
        public IEnumerable<CostingFormatEnum> CostingFormats { get; } = Enum.GetValues(typeof(CostingFormatEnum)).Cast<CostingFormatEnum>();

        #region Fields
        private int _id, _vNumber = -2;
        private string _qNumber;
        private Item _item;
        private Admin _costingPreparedBy;
        private RMCosting _rmCosting;
        private ConversionCosting _convCosting;
        private DateTime _date = DateTime.Now;
        private CostingCategoryEnum _costingCategory;
        #endregion Fields

        #region Properties
        public int Id { get { return _id; } set { _id = value; } }
        public int V_Number
        {
            get { return _vNumber; }
            set
            {
                if (_vNumber != value)
                {
                    _vNumber = value;
                }
            }
        }
        public string Q_Number
        {
            get { return _qNumber; }
            set
            {
                if (_qNumber != value)
                {
                    _qNumber = value;
                }
            }
        }
        public DateTime Date
        {
            get { return _date; }
            set 
            {
                if (_date != value)
                {
                    _date = value;
                }
            }
        }
        public Item Item { get { return _item;} set { _item = value; } }
        public Admin CostingPreparedBy { get { return _costingPreparedBy; } set { _costingPreparedBy = value; } }
        public RMCosting RMCosting  { get { return _rmCosting; } set { _rmCosting = value; } }
        public CostingCategoryEnum Category { 
            get { return _costingCategory; } 
            set { 
                _costingCategory = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        public ConversionCosting ConvCosting
        {
            get { return _convCosting; }
            set { _convCosting = value; }
        }

        #endregion Properties

        #region Methods

        public string GetQ_Number(int itemId)
        {
            return $"Q{itemId}V{V_Number}"; 
        }
        public bool IsDataValid()
        {
            if (RMCosting== null) { return false; }
            if (Item == null) { return false; }
            if (!RMCosting.IsDataValid()) { return false; }
            return true;
        }

        public int ComputeItemStatus()
        {
            if (((int)Category) == 1) { return ((int)(ItemStatusEnum.MF_Costing_Prepared)); }
            if (((int)Category) == 2) { return ((int)ItemStatusEnum.Customer_Costing_Prepared); }

            // TODO: Check if approved costing and PO recd are the same things
            if (((int)Category) == 3) { return ((int)ItemStatusEnum.POReceived); }
            return -1;
        }

        public float AddProfit()
        {
            return (float)((RMCosting.CostPerPiece + ConvCosting.TotalCostPerPiece) * 0.15);
        }

        public float ComputeTotalCost()
        {
            return RMCosting.CostPerPiece + ConvCosting.TotalCostPerPiece + AddProfit();
        }
        #endregion Methods
    }
}

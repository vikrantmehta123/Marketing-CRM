using Metaforge_Marketing.Models.Enums;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Metaforge_Marketing.Models
{
    public class Costing : ModelsBase
    {
        public IEnumerable<CostingCategoryEnum> CostingCategories { get; } = Enum.GetValues(typeof(CostingCategoryEnum)).Cast<CostingCategoryEnum>();
        public IEnumerable<CostingFormatEnum> CostingFormats { get; } = Enum.GetValues(typeof(CostingFormatEnum)).Cast<CostingFormatEnum>();

        #region Fields
        private int _id;
        private float _rmCostPerPiece, _ccPerPiece, _totalCostPerPiece;
        private bool _isRMCostingPresent, _isConvCostingPresent;
        private Item _item;
        private Admin _costingPreparedBy;
        private Remark _remark;
        private RMCosting _rmCosting;
        private List<Operation> _operations;
        private CostingCategoryEnum _costingCategory;
        private CostingFormatEnum _format;
        #endregion Fields

        #region Properties
        public int Id { get { return _id; } set { _id = value; } }
        public Item Item { get { return _item;} set { _item = value; } }
        public Admin CostingPreparedBy { get { return _costingPreparedBy; } set { _costingPreparedBy = value; } }
        public Remark Remark { get { return _remark; } set { _remark = value; } }
        public RMCosting RMCosting 
        { 
            get 
            {   
                return _rmCosting; 
            } 
            set 
            { 
                _rmCosting = value; 
            } 
        }
        public List<Operation> Operations { get { return _operations; } set { _operations = value;} }
        public CostingCategoryEnum Category { get { return _costingCategory; } set { _costingCategory = value; } }
        public CostingFormatEnum Format { 
            get { return _format; } 
            set { 
                _format = value; 
                OnPropertyChanged(nameof(Format));

            } 
        }

        public float CCPerPiece
        {
            get
            {
                _ccPerPiece = _operations.Sum(item => item.CostPerPiece);
                return _ccPerPiece;
            }
        }

        #endregion Properties


        public Costing()
        {
            _operations = new List<Operation>();
        }

        #region Methods
        public bool IsDataValid()
        {
            if (RMCosting== null) { return false; }
            if (Item == null) { return false; }
            if (!RMCosting.IsDataValid()) { return false; }
            if (Operations.Count == 0) { return false; }
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

        #endregion Methods
    }
}

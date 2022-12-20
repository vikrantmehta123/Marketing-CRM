using Metaforge_Marketing.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaforge_Marketing.Models
{
    public class Costing
    {
        public IEnumerable<CostingCategoryEnum> CostingCategories { get; } = Enum.GetValues(typeof(CostingCategoryEnum)).Cast<CostingCategoryEnum>();
        public IEnumerable<CostingFormatEnum> CostingFormats { get; } = Enum.GetValues(typeof(CostingFormatEnum)).Cast<CostingFormatEnum>();

        #region Fields
        private int _id;
        private Item _item;
        private RMCosting _rmCosting;
        private List<Operation> _operations;
        private CostingCategoryEnum _costingCategory;
        private CostingFormatEnum _costingFormat;
        #endregion Fields

        #region Properties
        public int Id { get { return _id; } set { _id = value; } }
        public Item Item { get { return _item;} set { _item = value; } }
        public RMCosting RMCosting { get { return _rmCosting; } set { _rmCosting = value; } }
        public List<Operation> Operations { get { return _operations; } set { _operations = value;} }
        public CostingCategoryEnum CostingCategory { get { return _costingCategory; } set { _costingCategory = value; } }
        public CostingFormatEnum CostingFormat { get { return _costingFormat; } set { _costingFormat = value; } }
        #endregion Properties
    }
}

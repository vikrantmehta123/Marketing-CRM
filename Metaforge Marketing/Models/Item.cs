
using Metaforge_Marketing.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaforge_Marketing.Models
{
    public class Item : ModelsBase
    {

        public static IEnumerable<OrderTypeEnum> OrderTypes { get; } = Enum.GetValues(typeof(OrderTypeEnum)).Cast<OrderTypeEnum>();
        public static IEnumerable<PriorityEnum> Priorities { get; } = Enum.GetValues(typeof(PriorityEnum)).Cast<PriorityEnum>();

        #region Fields

        private int _id, _status, _qty;
        private string _itemName, _itemCode;
        private float _grossWeight, _netWeight;
        private bool _isRegretted, _isRejectedByCustomer, _isMFCostingPrepared, _isCustomerCostingPrepared, _isQuotationSent, _isPOReceived;
        private Admin _quotationHandledBy;
        private RFQ _rfq;
        private Customer _customer;
        private PriorityEnum _priority;
        private OrderTypeEnum _orderType;
        private ItemStatusEnum _itemStatus;
        #endregion Fields

        #region Properties
        public int Id { get { return _id; } set { _id = value; } }
        public int Status { get { return _status; } set { _status = value; } }
        public PriorityEnum Priority { get { return _priority; } set { _priority = value; } }
        public OrderTypeEnum OrderType { get { return _orderType; }  set { _orderType = value; } }
        public int Qty { get { return _qty; } set { _qty = value; } }
        public string ItemName { get { return _itemName;} set { _itemName = value; } }
        public string ItemCode { get { return _itemCode; } set { _itemCode = value; } }
        public float GrossWeight { get { return _grossWeight; } set { _grossWeight = value; } }
        public float NetWeight { get { return _netWeight; } set { _netWeight = value; } }
        public Customer Customer { get { return _customer; } set { _customer = value; } }
        public RFQ RFQ { get { return _rfq; } set { _rfq = value;}}
        #endregion Properties

        #region Boolean Indicator Properties
        public bool IsRegretted { get { return _isRegretted;}  }
        public bool IsMFCostingPrepared { get { return _isMFCostingPrepared; } }
        public bool IsCustomerCostingPrepared { get { return _isCustomerCostingPrepared; } }
        public bool IsRejected { get { return _isRejectedByCustomer; } set { _isRejectedByCustomer = value; } }
        public bool IsQuotationSent { get { return _isQuotationSent; } set { _isQuotationSent = value; } }
        public bool IsPOReceived { get { return _isPOReceived; } set { _isPOReceived = value; } }

        #endregion Boolean Indicator Properties
    }
}

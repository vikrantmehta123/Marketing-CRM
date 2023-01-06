
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
        private int _id, _qty;
        private string _itemName, _itemCode, _note, _reason;
        private float _grossWeight, _netWeight;
        private bool _isRegretted;
        private Admin _quotationHandledBy;
        private RFQ _rfq;
        private Customer _customer;
        private RejectRegretEnum _rejectRegret;
        private PriorityEnum _priority;
        private OrderTypeEnum _orderType;
        private ItemStatusEnum _itemStatus;
        private DateTime _eventDate = DateTime.Today.Date;
        #endregion Fields

        #region Properties
        public int Id { get { return _id; } set { _id = value; } }
        public ItemStatusEnum Status { get { return _itemStatus; } set { _itemStatus = value; } }
        public PriorityEnum Priority { get { return _priority; } set { _priority = value; } }
        public OrderTypeEnum OrderType { get { return _orderType; }  set { _orderType = value; } }
        public int Qty { get { return _qty; } set { _qty = value; } }
        public bool IsRegretted { get { return _isRegretted; } set { _isRegretted = value; } }
        public string ItemName { get { return _itemName;} set { _itemName = value; } }
        public string ItemCode { get { return _itemCode; } set { _itemCode = value; } }
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }
        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }
        public DateTime EventDate
        {
            get { return _eventDate; }
            set { _eventDate = value; }
        }
        public float GrossWeight { get { return _grossWeight; } set { _grossWeight = value; } }
        public float NetWeight { get { return _netWeight; } set { _netWeight = value; } }
        public Customer Customer { get { return _customer; } set { _customer = value; } }
        public Admin QuotationHandledBy
        {
            get { return _quotationHandledBy; }
            set { _quotationHandledBy = value; }
        }
        public RFQ RFQ { get { return _rfq; } set { _rfq = value;}}

        public RejectRegretEnum RejectRegret
        {
            get { return _rejectRegret; }
            set { _rejectRegret = value; }
        }
        #endregion Properties



        #region Methods
        public string GetNote(ItemStatusEnum status)
        {
            if (String.IsNullOrEmpty(_note))
            {
                string Note;
                if (status == ItemStatusEnum.MF_Costing_Prepared) { Note = "MF Costing prepared"; }
                else if (status == ItemStatusEnum.Customer_Costing_Prepared) { Note = "Customer Costing prepared"; }
                else { Note = "Approved costing entered"; }
                return Note;
            }
            else { return _note; }
        }
        
        public bool IsFormEntryValid()
        {
            if (GrossWeight == 0) { return false; }
            if (NetWeight== 0) { return false; }
            if(String.IsNullOrEmpty(ItemCode)) { return false; }
            if(String.IsNullOrEmpty(ItemName)) { return false; }
            return true;
        }
        #endregion Methods
    }
}

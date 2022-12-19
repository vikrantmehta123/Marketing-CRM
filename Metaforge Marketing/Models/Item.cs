
namespace Metaforge_Marketing.Models
{
    public class Item
    {
        #region Fields
        private int _id, _rfqId, _status, _priority, _qtyId;
        private string _itemName, _itemCode;
        private float _grossWeight, _netWeight;
        #endregion Fields

        #region Properties
        public int Id { get { return _id; } set { _id = value; } }
        public int Status { get { return _status; } set { _status = value; } }
        public string ItemName { get { return _itemName;} set { _itemName = value; } }
        public string ItemCode { get { return _itemCode; } set { _itemCode = value; } }
        public float GrossWeight { get { return _grossWeight; } set { _grossWeight = value; } }
        public float NetWeight { get { return _netWeight; } set { _netWeight = value; } }
        #endregion Properties
    }
}

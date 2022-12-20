

using Metaforge_Marketing.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metaforge_Marketing.Models
{
    public class RMCosting
    {
        #region Fields
        private int _id;
        private string _rmAsPerDrawing, _rmConsidered;
        private float _rmRate, _scrapRate, _scrapRecovery, _cuttingAllowance, _itemLength;
        #endregion Fields

        #region Properties
        public int Id { get { return _id; } set { _id = value; } }
        public string RMAsPerDrawing { get { return _rmAsPerDrawing; } set { _rmAsPerDrawing = value;} }
        public string RMConsidered { get { return _rmConsidered;} set { _rmConsidered = value;} }
        public float RMRate { get { return _rmRate; } set { _rmRate= value; } }
        public float ScrapRate { get { return _scrapRate; } set { _scrapRate= value; } }
        public float CuttingAllowance { get { return _cuttingAllowance; } set { _cuttingAllowance = value; } }
        public float ItemLength { get { return _itemLength; } set { _itemLength = value; } }
        public float ScrapRecovery { get { return _scrapRecovery; } set { _scrapRecovery= value; } }
        #endregion Properties
    }
}

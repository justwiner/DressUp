using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class StorageOrderSVM
    {
        public string GoodsName { get; set; }
        public string MessageType { get; set; }
        public int Num { get; set; }
        public Guid MessageId { get; set; }
        public decimal GoodsPurchasePrice { get; set; }
        public string GoodsType { get; set; }
        public Guid GoodsId { get; set; }
    }
}
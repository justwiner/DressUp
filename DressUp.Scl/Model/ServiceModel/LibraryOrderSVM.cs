using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class LibraryOrderSVM
    {
        public Guid MessageId { get; set; }
        public string MessageType { get; set; }
        public string GoodsName { get; set; }
        public string GoodsType { get; set; }
        public string ContactInfo { get; set; }
        public Guid OrderId { get; set; }
        public decimal GoodsPurchasePrice { get; set; }
        public int GoodsNum { get; set; }
        public string Consignee { get; set; }
        public string ReceiptAddress { get; set; }
        public Guid GoodsId { get; set; }
    }
}
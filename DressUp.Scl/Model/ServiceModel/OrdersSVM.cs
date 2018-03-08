using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class OrdersSVM
    {
        public Guid OrderNum { get; set; }
        public string OrderStatus { get; set; }
        public string GoodsImg { get; set; }
        public string GoodsName { get; set; }
        public decimal GoodsPrice { get; set; }
        public int GoodsNum { get; set; }
        public string OrderGenerationTime { get; set; }
        public string ReceiptAddress { get; set; }
        public string Consignee { get; set; }
        public string ContactInfo { get; set; }
    }
}
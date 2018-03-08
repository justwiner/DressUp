using System;

namespace DressUp.Scl.Model.ViewModel
{
    public class OrdersVM
    {
        public Guid OrderNum { get; set; }
        public string OrderStatus { get; set; }
        public Guid UserId { get; set; }
        public Guid GoodsId { get; set; }
        public string GoodsName { get; set; }
        public decimal GoodsPrice { get; set; }
        public int GoodsNum { get; set; }
        public string OrderGenerationTime { get; set; }
        public string ReceiptAddress { get; set; }
        public string Consignee { get; set; }
        public string ContactInfo { get; set; }
    }
}
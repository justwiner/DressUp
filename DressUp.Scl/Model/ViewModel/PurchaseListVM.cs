using System;

namespace DressUp.Scl.Model.ViewModel
{
    public class PurchaseListVM
    {
        public Guid OrderNum { get; set; }
        public string OrderType { get; set; }
        public Guid? GoodsId { get; set; }
        public string GoodsName { get; set; }
        public int StorageNum { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
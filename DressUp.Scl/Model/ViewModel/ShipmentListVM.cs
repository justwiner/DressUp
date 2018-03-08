using System;

namespace DressUp.Scl.Model.ViewModel
{
    public class ShipmentListVM
    {
        public Guid OrderId { get; set; }
        public string Type { get; set; }
        public Guid OrderNum { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
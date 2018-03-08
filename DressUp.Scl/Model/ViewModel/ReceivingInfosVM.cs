using System;

namespace DressUp.Scl.Model.ViewModel
{
    public class ReceivingInfosVM
    {
        public Guid UserId { get; set; }
        public string Consignee { get; set; }
        public string ReceiptAddress { get; set; }
        public string ContactInfo { get; set; }
        public bool IsDefault { get; set; }
        public int ReceivingInfoId { get; set; }
    }
}
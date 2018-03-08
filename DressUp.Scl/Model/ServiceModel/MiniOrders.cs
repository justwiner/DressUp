using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class MiniOrders
    {
        public Guid GoodsId { get; set; }
        public int GoodsNum { get; set; }
        public int ReceivingInfoId { get; set; }
        public int ShopCartsId { get; set; }

    }
}
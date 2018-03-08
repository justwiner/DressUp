using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class SimpleOrdersSVM
    {
        public Guid OrderNum { get; set; }
        public Guid GoodsId { get; set; }
        public string GoodsImg { get; set; }
        public string GoodsName { get; set; }
        public decimal? GoodsPrice { get; set; }
        public int GoodsNum { get; set; }
        public decimal? GoodsTotalPrice { get; set; }
        public int ShopCartsId { get; set; }
    }
}
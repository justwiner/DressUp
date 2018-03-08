using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class GoodsSVM
    {
        public Guid GoodsId { get; set; }
        public string GoodsName { get; set; }
        public int ClassificationId { get; set; }
        public string GoodsSimpleGraph { get; set; }
        public string GoodsDetail { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal? Price { get; set; }
        public decimal? PromotionPrice { get; set; }
        public string GoodsStatus { get; set; }
        public int Stock { get; set; }
        public int? ShelvesNum { get; set; }
        public bool? IfNewGoods { get; set; }
        public bool? IfHot { get; set; }
        public string OnShelfTime { get; set; }
        public string OffShelfTime { get; set; }
        public int GoodsSaleNum { get; set; }
        public string GoodsTypeName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ViewModel
{
    public class GoodsVM
    {
        public Guid GoodsId { get; set; }
        public string GoodsName { get; set; }
        public int ClassificationId { get; set; }
        public string GoodsSimpleGraph { get; set; }
        public string GoodsDetail { get; set; }
        public decimal PurchasePrice{ get; set; }
        public decimal? Price { get; set; }
        public decimal? PromotionPrice { get; set; }
        public string GoodsStatus { get; set; }
        public int Stock { get; set; }
        public int? ShelvesNum { get; set; }
        public bool? IfNewGoods { get; set; }
        public bool? IfHot { get; set; }
        public DateTime? OnShelfTime { get; set; }
        public DateTime? OffShelfTime { get; set; }
        public int GoodsSaleNum { get; set; }
    }
}
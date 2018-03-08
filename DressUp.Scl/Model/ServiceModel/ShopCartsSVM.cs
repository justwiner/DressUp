using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class ShopCartsSVM
    {
        public Guid UserId { get; set; }
        public int GoodsNum { get; set; }
        public int ShoppingCartId { get; set; }
        public string GoodsSimpleGraph { get; set; }
        public Guid GoodsId { get; set; }
        public string GoodsName { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
        public int SerialNumber { get; set; }
    }
}
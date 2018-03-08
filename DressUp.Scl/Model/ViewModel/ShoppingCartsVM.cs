using System;

namespace DressUp.Scl.Model.ViewModel
{
    public class ShoppingCartsVM
    {
        public Guid UserId { get; set; }
        public int GoodsNum { get; set; }
        public DateTime AddTime { get; set; }
        public string GoodsStatus { get; set; }
        public int ShoppingCartId { get; set; }
        public Guid GoodsId { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DressUp_Scl_Data.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Goods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Goods()
        {
            this.GoodsAtlas = new HashSet<GoodsAtlas>();
        }
    
        public System.Guid GoodsId { get; set; }
        public string GoodsName { get; set; }
        public int ClassificationId { get; set; }
        public string GoodsSimpleGraph { get; set; }
        public string GoodsDetail { get; set; }
        public decimal PurchasePrice { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> PromotionPrice { get; set; }
        public string GoodsStatus { get; set; }
        public int Stock { get; set; }
        public Nullable<int> ShelvesNum { get; set; }
        public Nullable<bool> IfNewGoods { get; set; }
        public Nullable<bool> IfHot { get; set; }
        public Nullable<System.DateTime> OnShelfTime { get; set; }
        public Nullable<System.DateTime> OffShelfTime { get; set; }
        public int GoodsSaleNum { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsAtlas> GoodsAtlas { get; set; }
    }
}

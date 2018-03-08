using System.Collections.Generic;

namespace DressUp_Scl_Service.Model
{
    public class GoodsTypes_
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int FatherTypeId { get; set; }
        public List<GoodsTypes_> Children { get; set; }
    }
}

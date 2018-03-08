using DressUp.Scl.Model.ViewModel;
using System.Collections.Generic;

namespace DressUp.Scl.Model.ServiceModel
{
    public class GoodsAndAtlasSVM
    {
        public GoodsSVM Goods { get; set; }
        public List<GoodsAtlasVM> Atlas { get; set; }
    }
}
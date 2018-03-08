using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ViewModel
{
    public class GoodsTypesVM
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int FatherTypeId { get; set; }
        public string TypeEnglishName { get; set; }
    }
}
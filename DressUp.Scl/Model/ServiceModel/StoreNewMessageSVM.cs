using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class StoreNewMessageSVM
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public Guid? GoodsId { get; set; }
        public string GoodsName { get; set; }
        public int Num { get; set; }
    }
}
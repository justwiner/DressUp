using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class UserSVM
    {
        public Guid UserId { get; set; }
        public string BuyerName { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public DateTime CreationTime { get; set; }
        public string ContactInfo { get; set; }
        public int ShoppingCartsCount { get; set; }
    }
}
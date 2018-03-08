using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ServiceModel
{
    public class UsersInfoSVM
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public int WaitReceivingOrdersNum { get; set; }
    }
}
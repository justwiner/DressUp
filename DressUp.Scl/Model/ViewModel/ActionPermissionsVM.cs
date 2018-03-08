using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ViewModel
{
    public class ActionPermissionsVM
    {
        public System.Guid ActionId { get; set; }
        public System.Guid PermissionId { get; set; }
        public string ActionName { get; set; }
    }
}
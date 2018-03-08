using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DressUp.Scl.Model.ViewModel
{
    public class MenusVM
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int FatherMenuId { get; set; }
        public string PermissionCode { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIcon { get; set; }
    }
}
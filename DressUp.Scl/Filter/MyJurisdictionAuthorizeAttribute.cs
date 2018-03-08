using DressUp_Scl_Data.Data;
using DressUp_Scl_Service.Service.BackStageService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DressUp.Scl.Filter
{
    public class MyJurisdictionAuthorizeAttribute: AuthorizeAttribute
    {
        private DressUpWebDbEntities db = new DressUpWebDbEntities();
        private PermissionsService permissionService = new PermissionsService();
        string actionName = "";

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Users user = HttpContext.Current.Session["User"] as Users;
            Permissions permission = GetActionPermissions(actionName);
            foreach (Permissions item in permissionService.GetPermissions(user))
            {
                if (item.PermissionId == permission.PermissionId)
                {
                    return true;
                }
            }
            return false;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            actionName = filterContext.ActionDescriptor.ActionName;
            base.OnAuthorization(filterContext);
        }
        public Permissions GetActionPermissions(string actionName)
        {
            Permissions permission = new Permissions();
            Guid permissionId = db.ActionPermissons.SingleOrDefault(m => m.ActionName == actionName).PermissionId;
            permission = db.Permissions.SingleOrDefault(m => m.PermissionId == permissionId);
            return permission;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.HttpContext.Response.RedirectPermanent("/HomePage/NotFound", false);
            filterContext.HttpContext.Response.Redirect("/HomePage/NotFound");
        }
    }
}
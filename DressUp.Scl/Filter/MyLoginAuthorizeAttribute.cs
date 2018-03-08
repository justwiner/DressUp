using DressUp_Scl_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DressUp.Scl.Filter
{
    public class MyLoginAuthorizeAttribute: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Users user = HttpContext.Current.Session["User"] as Users;
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.HttpContext.Response.RedirectPermanent("/HomePage/BackLogPage", false);
            filterContext.HttpContext.Response.Redirect("/HomePage/BackLogPage");
        }
    }
}
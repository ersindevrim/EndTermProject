using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShareYourNote.WebUI.Models;

namespace ShareYourNote.WebUI.Filters
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionManager.User != null && SessionManager.User.IsAdmin == false)
            {
                filterContext.Result=new RedirectResult("/Home/AccessDenied");
            }
        }
    }
}
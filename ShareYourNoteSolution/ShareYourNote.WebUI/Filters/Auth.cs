using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShareYourNote.WebUI.Models;

namespace ShareYourNote.WebUI.Filters
{
    public class Auth : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionManager.User == null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using InstagramWeb.Controllers;
using InstagramWeb.Models;

namespace InstagramWeb.Filters
{
    public class RoleAttribute : ActionFilterAttribute
    {
        public List<int> type { get; set; }
        public RoleAttribute(int type)
        {
            this.type = new List<int>
            {
                type
            };
        }
        public RoleAttribute(int[] type)
        {
            this.type = type.ToList();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (PanelController)filterContext.Controller;
            SessionUser user = controller.Session["User"] as SessionUser;
            if (user != null)
            {
                if (!this.type.Contains(user.RoleId))
                {
                    if (controller.Request.Cookies["KMGQQMK"] != null)
                    {
                        var c = controller.Request.Cookies["KMGQQMK"];
                        c.Expires = DateTime.Now.AddDays(-1);
                        controller.Response.Cookies.Add(c);
                    }
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Permissions" }, { "controller", "Account" } });
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

    }
}
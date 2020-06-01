using System.Web.Mvc;
using InstagramWeb.Models;

namespace TicketsSale.Filters
{
    public class RequireSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (Controller)filterContext.Controller;
            SessionUser _user = controller.Session["User"] as SessionUser; 
            if (_user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Login" }, { "controller", "Account" } });
                return;
            }
            //if (!_user.EmailVerified)
            //{
            //    string actionName = filterContext.ActionDescriptor.ActionName;
            //    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            //    if (actionName != "EmailVerification" && controllerName != "Account")
            //    {
            //        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "EmailVerification" }, { "controller", "Account" } });
            //        return;
            //    }
            //}
            //if (_user.InActive)
            //{
            //    string actionName = filterContext.ActionDescriptor.ActionName;
            //    string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //    if (actionName != "Blocked")
            //    {
            //        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Blocked" }, { "controller", "Account" } });
            //        return;
            //    }
            //}
        }
    }
}
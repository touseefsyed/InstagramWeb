using System.Web.Mvc;
using InstagramWeb.Controllers;
using InstagramWeb.Models;

namespace InstagramWeb.Filters
{
    public class CookieLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (BaseController)filterContext.Controller;
            SessionUser _user = controller.Session["User"] as SessionUser;
            if (_user == null)
            {
                if (controller.Request.Cookies["LoginData"] != null)
                {

                }
            }
        }
    }
}
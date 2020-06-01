//using System.Web.Mvc;
//using TicketsSale.Controllers;
//using TicketsSale.Models;

//namespace TicketsSale.Filters
//{
//    public class ValidatorAttribute : ActionFilterAttribute
//    {
        
//        public ValidatorAttribute()
//        {
//        }
//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            var controller = (PanelController)filterContext.Controller;
//            string actionName = filterContext.ActionDescriptor.ActionName;
//            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
//            var user = controller.Session["User"] as SessionUser;
        

//            //if (WebRepo.DateExpired() && filterContext.ActionDescriptor.ActionName != "Expired" && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Error") 
//            //{
//            //    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Expired" }, { "controller", "Error" } });
//            //}
//        }
//    }
//}
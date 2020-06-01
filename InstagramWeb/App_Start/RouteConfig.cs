using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InstagramWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Profile",
                url: "User/Profile",
                defaults: new { controller = "User", action = "AddUser", Profile = true }
            );

            routes.MapRoute(
                name: "AddUser",
                url: "User/AddUser",
                defaults: new { controller = "User", action = "AddUser", Profile = false }
            );

            routes.MapRoute(
                name: "AddProxy",
                url: "Proxy/AddProxy",
                defaults: new { controller = "Proxy", action = "AddProxy" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}

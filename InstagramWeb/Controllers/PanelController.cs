using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstagramWeb.Models;
using TicketsSale.Filters;

namespace InstagramWeb.Controllers
{
    [RequireSession]
    public class PanelController : BaseController
    {
        public SessionUser user => Session["User"] as SessionUser;

        public override void Notify(NotificationMessage message)
        {
            Notify(user.ConnectionIdentifier, message);
        }

        public void Notify(string connectionIdentifier, NotificationMessage message)
        {
           
            if (message.IsRedirectMessage)
            {
                TempData["NotificationMessage"] = message;
            }
            if (message.IsViewMessage)
            {
                ViewBag.NotificationMessage = message;
            }
        }
    }
}
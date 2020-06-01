using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstagramWeb.Models;

namespace InstagramWeb.Controllers
{
    public class BaseController : Controller
    {
        public ScheduleContext ScheduleContext;
        public BaseController()
        {
            ScheduleContext = new ScheduleContext();

        }

        public virtual void Notify(NotificationMessage message)
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

        public void Notify(string Type, string Title, string Description, bool IsAjaxMessage = true, bool IsViewMessage = true, bool IsRedirectMessage = false)
        {
            Notify(new NotificationMessage
            {
                MessageType = Type,
                Title = Title,
                Description = Description,
                IsAjaxMessage = IsAjaxMessage,
                IsViewMessage = IsViewMessage,
                IsRedirectMessage = IsRedirectMessage
            });
        }

    }
}
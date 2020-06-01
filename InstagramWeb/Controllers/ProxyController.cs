using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using InstagramWeb.Filters;
using InstagramWeb.Models;
using InstagramWeb.Models.ViewModels;

namespace InstagramWeb.Controllers
{
    using Roles = Models.Roles;
    [Role((int)Roles.Admin)]
    public class ProxyController : PanelController
    {
        [HttpGet]
        public ActionResult ProxyList()
        {
            var ProxyList = ScheduleContext.Proxies.ToList();
            return View( ProxyList.ToList() );
        }


        [HttpGet]
        public ActionResult AddProxy(int Id = 0)
        {
            var proxy = new Proxy();
            if (Id != 0)
            {
                proxy = ScheduleContext.Proxies.FirstOrDefault(x => x.Id == Id);
            
            }
            return View(proxy);
        }

        [HttpPost]
        public ActionResult AddProxy(Proxy proxy)
        {
            if (ModelState.IsValid)
            {
                ScheduleContext.Proxies.AddOrUpdate(proxy);
                ScheduleContext.SaveChanges();
            }

            if (proxy.Id == 0)
            {
                Notify("Success", "Successfully Added", "Proxy Added Successfully");
            }
            else
            {
                Notify("Success", "Successfully Updated", "Proxy Updated Successfully");

            }
            return RedirectToAction("AddProxy", "Proxy");
        }


        [HttpPost]
        public bool DeleteProxy(int ProxyId)
        {
            var Proxy = ScheduleContext.Proxies.FirstOrDefault(x => x.Id == ProxyId);
            if (Proxy != null)
            {
                ScheduleContext.Proxies.Remove(Proxy);
                ScheduleContext.SaveChanges();
            }
            Notify("Success", "Successfully Deleted", "Proxy Deleted Successfully", IsRedirectMessage: true);
            return true;
        }
    }
}
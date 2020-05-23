using InstagramWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InstagramWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        ScheduleContext _db = new ScheduleContext();
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult AddSchedule()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddSchedule(Schedule obj)
        {
            ScheduleContext _db = new ScheduleContext();



            if (ModelState.IsValid)
            {


                _db.Schedules.Add(obj);
                _db.SaveChanges();

            }

            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
           
                return View();
        }
    }
}

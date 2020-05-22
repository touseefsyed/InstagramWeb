using InstagramWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstagramWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
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
            return View();
          }

        }
    }

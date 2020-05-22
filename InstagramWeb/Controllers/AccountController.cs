using InstagramWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstagramWeb.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User obj)
        {

            string UserName = "touseef";
            string Password = "123";

            if (obj.UserName == UserName && obj.Password == Password)
            {
                Session["UserName"] = obj.UserName.ToString();
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
               ViewBag.Error = "Please Enter Valid User Name or Password";
                return View("Login");
            }
          
        }
    }
}
using InstagramWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstagramWeb.Models.ViewModels;


namespace InstagramWeb.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginRequest request)
        {

            var userQuery = ScheduleContext.Users.AsQueryable();
            var userRequest = userQuery.Include(x=>x.Proxy).FirstOrDefault(x => x.Username == request.Username && x.Password == request.Password);
            if (userRequest != null)
            {
                Session["User"] = new SessionUser(userRequest);
                return RedirectToAction("ScheduleList", "Schedule");
            }
            Notify("Error", "Invalid Credentials", "Your credentials are invalid");
            return View();
        }
  

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
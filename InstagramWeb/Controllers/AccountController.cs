using InstagramWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstagramWeb.Models.ViewModels;
using InstagramWeb.Migrations;

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
                return RedirectToAction("Index", "Dashboard");
            }
            Notify("Error", "Invalid Credentials", "Your credentials are invalid", IsRedirectMessage:true);
            return RedirectToAction("Login", "Account");
        }
  

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }



        [HttpGet]
        public ActionResult Authorize(string code, string state)
        {
            var user = ScheduleContext.Users.FirstOrDefault(x => x.Username == state);
            user.InstagramAuthToken = code;
            ScheduleContext.SaveChanges();
            if (Session["User"] == null)
            {
                Session["User"] = new SessionUser(user);
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
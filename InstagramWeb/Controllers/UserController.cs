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
using System.Data.Entity;

namespace InstagramWeb.Controllers
{
    using Roles = Models.Roles;
    public class UserController : PanelController
    {

        [HttpGet]
        [Role((int)Roles.Admin)]
        public ActionResult UserException(DailyFollowerCountFilter dailyFollowerCountFilter)
        {
            if(dailyFollowerCountFilter == null)
            {
                dailyFollowerCountFilter = new DailyFollowerCountFilter();
            }

            var dailyFollowerCount = ScheduleContext.DailyFollowerCount.Include(x=>x.User).AsQueryable();
            if (!string.IsNullOrWhiteSpace(dailyFollowerCountFilter.Exception))
            {
                dailyFollowerCount = dailyFollowerCount.Where(x => x.Exception.Contains(dailyFollowerCountFilter.Exception));
            }
            if (dailyFollowerCountFilter.Followers >= 0)
            {
                dailyFollowerCount = dailyFollowerCount.Where(x => x.Followers == dailyFollowerCountFilter.Followers);
            }
            if (dailyFollowerCountFilter.RecordedDateFrom != null)
            {
                dailyFollowerCount = dailyFollowerCount.Where(x => x.RecordedDate >= dailyFollowerCountFilter.RecordedDateFrom);

            }
            if (dailyFollowerCountFilter.RecordedDateTo != null)
            {
                dailyFollowerCount = dailyFollowerCount.Where(x => x.RecordedDate <= dailyFollowerCountFilter.RecordedDateTo);
            }
            if (dailyFollowerCountFilter.UserId != null)
            {
                dailyFollowerCount = dailyFollowerCount.Where(x => x.UserId <= dailyFollowerCountFilter.UserId);
            }

            return View(new UserExceptionViewModel {Users =ScheduleContext.Users.ToList(),  DailyFollowerCountFilter = dailyFollowerCountFilter, DailyFollowerCount = dailyFollowerCount.OrderByDescending(x=>x.RecordedDate).ToList() });
        }



        [HttpGet]
        [Role((int)Roles.Admin)]
        public ActionResult UserList(User UserFilter)
        {
            if (UserFilter == null)
            {
                UserFilter = new User();
            }

            var userList = ScheduleContext.Users.AsQueryable();
            if (UserFilter.RoleId != 0)
            {
                userList = userList.Where(x => x.RoleId == UserFilter.RoleId);
            }

            if (!string.IsNullOrWhiteSpace(UserFilter.FirstName))
            {
                userList = userList.Where(x => x.FirstName.Contains(UserFilter.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(UserFilter.LastName))
            {
                userList = userList.Where(x => x.LastName.Contains(UserFilter.LastName));
            }

            if (!string.IsNullOrWhiteSpace(UserFilter.Email))
            {
                userList = userList.Where(x => x.Email.Contains(UserFilter.Email));
            }

            return View(new UserListViewModel { UserFilter = UserFilter, Users = userList.ToList() });
        }


        [HttpGet]
        public ActionResult AddUser(int Id = 0, bool Profile = false)
        {
            var proxies = ScheduleContext.Proxies.ToList();
            
            ViewBag.Profile = Profile;
            if (Profile)
            {
                Id = user.Id;
            }

            var userVm = new RegisterRequest();
            userVm.Proxies = proxies;
            if (Id != 0)
            {
                var curUser = ScheduleContext.Users.FirstOrDefault(x => x.Id == Id);
                userVm = new RegisterRequest
                {
                    Username = curUser.Username,
                    LastName = curUser.LastName,
                    ConfirmPassword = curUser.Password,
                    Password = curUser.Password,
                    FirstName = curUser.FirstName,
                    RoleId = curUser.RoleId,
                    Email = curUser.Email,
                    ImagePath = curUser.ImagePath,
                    Id = curUser.Id,
                    ProxyId = curUser.ProxyId,
                    InstagramUsername = curUser.InstagramUsername,
                    InstagramPassword = curUser.InstagramPassword,
                    Proxies =  proxies
                };
            }
            return View(userVm);
        }

        [HttpPost]
        public ActionResult AddUser(RegisterRequest UserVm, bool Profile = false)
        {
            int userId = 0;
            if (Profile)
            {
                UserVm.RoleId = user.RoleId;
                UserVm.Id = user.Id;
            }

            if (ModelState.IsValid)
            {
                if (UserVm.ImageFile != null)
                {
                    string fn = Path.GetFileName(UserVm.ImageFile.FileName);
                    string fileExtension = fn.Remove(0, fn.IndexOf('.') + 1);
                    fn = UserVm.Id + "_." + fileExtension;
                    string SaveLocation = "~/Public/Files/ProfileImages/";
                    UserVm.ImagePath = SaveLocation + fn;
                    string FilePath = Server.MapPath(SaveLocation);
                    UserVm.ImageFile.SaveAs(Path.Combine(FilePath, fn));
                }
                var u = RegisterRequest.ToUser(UserVm);
                u.EmailVerified = true;
                ScheduleContext.Users.AddOrUpdate(x=>x.Id, u);

                userId = u.Id;
                ScheduleContext.SaveChanges();
            }
            if (Profile)
            {
                Session["User"] = new SessionUser(ScheduleContext.Users.FirstOrDefault(x=>x.Id == user.Id));
            }
            if (UserVm.Id == 0)
            {
                Notify("Success", "Successfully Added", "User Added Successfully", IsRedirectMessage:true);
            }
            else
            {
                Notify("Success", "Successfully Updated", "User Updated Successfully", IsRedirectMessage: true);

            }

            if (Profile)
            {

                return RedirectToRoute("Profile");
            }
            else
            {
                return RedirectToRoute("AddUser", new { ID=  userId});
            }

        }


        [HttpPost]
        [Role((int)Roles.Admin)]
        public bool DeleteUser(int UserId)
        {
            var user = ScheduleContext.Users.FirstOrDefault(x => x.Id == UserId);
            if (user != null)
            {
                ScheduleContext.Users.Remove(user);
                ScheduleContext.SaveChanges();
            }
            Notify("Success", "Successfully Deleted", "User Deleted Successfully", IsRedirectMessage: true);
            return true;
        }

        [HttpPost]
        [Role((int)Roles.Admin)]
        public bool DeleteException(int ExceptionId)
        {
            var user = ScheduleContext.DailyFollowerCount.FirstOrDefault(x => x.Id == ExceptionId);
            if (user != null)
            {
                ScheduleContext.DailyFollowerCount.Remove(user);
                ScheduleContext.SaveChanges();
            }
            Notify("Success", "Successfully Deleted", "Record Deleted Successfully", IsRedirectMessage: true);
            return true;
        }

        [HttpPost]
        [Role((int)Roles.Admin)]
        public bool ToggleBlockUser(int UserId)
        {
            var user = ScheduleContext.Users.FirstOrDefault(x => x.Id == UserId);
            if (user != null)
            {
                user.InActive = !user.InActive;
                ScheduleContext.SaveChanges();
            }
            Notify("Success", "Successfully Deleted", "User Deleted Successfully", IsRedirectMessage: true);
            return true;
        }


    }
}
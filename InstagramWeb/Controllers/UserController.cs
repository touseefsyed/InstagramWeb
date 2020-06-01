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
    public class UserController : PanelController
    {
        [HttpGet]
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
                var user = ScheduleContext.Users.FirstOrDefault(x => x.Id == Id);
                

                userVm = new RegisterRequest
                {
                    Username = user.Username,
                    LastName = user.LastName,
                    ConfirmPassword = user.Password,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    RoleId = user.RoleId,
                    Email = user.Email,
                    ImagePath = user.ImagePath,
                    Id = user.Id,
                    ProxyId = user.ProxyId,
                    InstagramUsername = user.InstagramUsername,
                    InstagramPassword = user.InstagramPassword,
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
            }

            if (ModelState.IsValid)
            {
                if (UserVm.ImageFile != null)
                {
                    string fn = Path.GetFileName(UserVm.ImageFile.FileName);
                    string fileExtension = fn.Remove(0, fn.IndexOf('.') + 1);
                    fn = UserVm.Id + "_." + fileExtension;
                    string SaveLocation = "~/Public/Files/ProfileImages";
                    UserVm.ImagePath = Path.Combine(SaveLocation, fn);
                    string FilePath = Server.MapPath(SaveLocation);
                    UserVm.ImageFile.SaveAs(Path.Combine(FilePath, fn));
                }
                var u = RegisterRequest.ToUser(UserVm);
                u.EmailVerified = true;
                ScheduleContext.Users.AddOrUpdate(x=>x.Id, u);

                userId = u.Id;
                ScheduleContext.SaveChanges();
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
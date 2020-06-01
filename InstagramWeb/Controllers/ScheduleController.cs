using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Hangfire;
using InstagramWeb.Models;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using PuppeteerSharp.Mobile;

namespace InstagramWeb.Controllers
{
    public class ScheduleController : PanelController
    {
        [HttpGet]
        public ActionResult ScheduleList()
        {
            var scheduleList = new List<Schedule>();
            if (user.RoleId == (int) Roles.Admin)
            {
                scheduleList = ScheduleContext.Schedules.ToList();
            }
            else
            {
                scheduleList = ScheduleContext.Schedules.Where(x=>x.UserId == user.Id).ToList();
            }
            return View(scheduleList);
        }

        public ActionResult AddSchedule(int Id = 0)
        {
            
            var schedule = new Schedule();
            var userList = ScheduleContext.Users.ToList();
            ViewBag.UserList = userList;
            if (Id != 0)
            {
                if (user.RoleId == (int)Roles.User)
                {
                    schedule = ScheduleContext.Schedules.FirstOrDefault(x => x.Id == Id && x.UserId == user.Id );
                }
                else
                {
                    schedule = ScheduleContext.Schedules.FirstOrDefault(x => x.Id == Id);
                }

            }
            return View(schedule);
        }
        [HttpPost]
        public ActionResult AddSchedule(Schedule obj)
        {

            string mode = "";
            if (obj.Id == 0)
            {
                mode = "insert";
            }
            else
            {
                mode = "update";
            }

            if (ModelState.IsValid)
            {
                ScheduleContext.Schedules.AddOrUpdate(obj);
                ScheduleContext.SaveChanges();
                // Check if Image File is not null
                if (obj.ImageFile != null)
                {
                    // Create a filepath
                    string fn = Path.GetFileName(obj.ImageFile.FileName);
                    string fileExtension = fn.Remove(0, fn.IndexOf('.') + 1);
                    fn = obj.Id + "_." + fileExtension;
                    string SaveLocation = "~/Posts/Files";
                    //Create folder if not exists. If permission issue. Create new folder
                    if (!Directory.Exists(Server.MapPath(SaveLocation)))
                    {
                        Directory.CreateDirectory(Server.MapPath(SaveLocation));
                    }
                    //Assign Path to Entity model
                    obj.ImagePath = Path.Combine(SaveLocation, fn);
                    string FilePath = Server.MapPath(SaveLocation);
                    obj.ImageFile.SaveAs(Path.Combine(FilePath, fn));
                }
                if (mode == "insert")
                {
                    string jobId = BackgroundJob.Schedule(() => AutomaticPost.AutomaticPosts(obj.Id), obj.TimeStamp);
                    obj.JobId = jobId;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(obj.JobId))
                    {
                        BackgroundJob.Delete(obj.JobId);
                        string jobId = BackgroundJob.Schedule(() => AutomaticPost.AutomaticPosts(obj.Id), obj.TimeStamp);
                        obj.JobId = jobId;
                    }
                }

                ScheduleContext.Schedules.AddOrUpdate(obj);
                ScheduleContext.SaveChanges();
                if (obj.Id == 0)
                {
                    Notify("Success", "Added Successfully", "Schedule Added Successfully", IsRedirectMessage: true);

                }
                else
                {
                    Notify("Success", "Updated Successfully", "Schedule Updated Successfully", IsRedirectMessage: true);

                }


            }
            return RedirectToAction("AddSchedule", "Schedule");
        }

        [HttpPost]
        public async  Task<ActionResult> SchedulePost(int ScheduleId)
        {
            await Schedule(ScheduleId);
            return RedirectToAction("AddSchedule", "Schedule");
        }

        private async Task Schedule(int ScheduleId)
        {
            await AutomaticPost.AutomaticPosts(ScheduleId);
            //string k = string.Empty;
            //try
            //{
            //    var scheduleQuery = ScheduleContext.Schedules.AsQueryable();
            //    var schedule = scheduleQuery.Include(x=>x.User).Include(x=>x.User.Proxy).FirstOrDefault(x => x.Id == ScheduleId);

            //    //104.148.46.2:3121
            //    var credentials = new InstagramCredential
            //    {
            //        Password = schedule.User.InstagramPassword,
            //        Email = schedule.User.Email
            //    };

            //    string Url = "https://www.instagram.com/";


            //    bool proxy = schedule.User.Proxy != null;
            //    var browerFetcher = new BrowserFetcher();
            //    await browerFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);

            //    //var path = Server.MapPath(browerFetcher.GetExecutablePath(BrowserFetcher.DefaultRevision));
            //    //k = path;
            //    using (var browser = Puppeteer.LaunchAsync(new LaunchOptions
            //    {
            //        Args = proxy ? new string[1] { $"--proxy-server={schedule.User.Proxy.IpAddress}" }: new string[0]{}, 
            //        ExecutablePath = browerFetcher.GetExecutablePath(BrowserFetcher.DefaultRevision),
            //        Headless = false,
            //        Timeout = 120000
            //    }).GetAwaiter().GetResult())
            //    {
            //        var devices = Puppeteer.Devices;
            //        var iphone = devices[DeviceDescriptorName.IPhone6];
            //        var page = browser.NewPageAsync().GetAwaiter().GetResult();
            //        await page.EmulateAsync(iphone);

            //        try
            //        {
            //            await page.GoToAsync(Url);
            //            string html = await page.GetContentAsync();
            //            System.Threading.Thread.Sleep(5000);
            //            await page.EvaluateExpressionAsync(AutomaticPost.ClickButton("Log In"));

            //            System.Threading.Thread.Sleep(5000);
            //            for (int i = 0; i < 4; i++)
            //            {
            //                await page.Keyboard.DownAsync(key: "Tab");
            //                System.Threading.Thread.Sleep(500);
            //            }

            //            await page.Keyboard.TypeAsync(credentials.Email, new TypeOptions {Delay = 200});
            //            await page.Keyboard.DownAsync(key: "Tab");
            //            await page.Keyboard.TypeAsync(credentials.Password, new TypeOptions {Delay = 200});
            //            await page.EvaluateExpressionAsync(AutomaticPost.ClickButton("Log In", true));
            //            System.Threading.Thread.Sleep(5000);
            //            await page.EvaluateExpressionAsync(AutomaticPost.ClickButton("Save Info"));
            //            System.Threading.Thread.Sleep(5000);
            //            await page.EvaluateExpressionAsync(AutomaticPost.ClickButton("Cancel"));
            //            System.Threading.Thread.Sleep(5000);
            //            //Dont add await for this
            //            page.EvaluateExpressionAsync(@"document.querySelector(""div[data-testid=new-post-button]"").click()");
            //            var fileChooser = await page.WaitForFileChooserAsync();
            //            await fileChooser.AcceptAsync(new string[] {Server.MapPath(schedule.ImagePath)});
            //            System.Threading.Thread.Sleep(5000);
            //            await page.EvaluateExpressionAsync(AutomaticPost.ClickButton("Next"));
            //            System.Threading.Thread.Sleep(5000);
            //            for (int i = 0; i < 4; i++)
            //            {
            //                await page.Keyboard.DownAsync(key: "Tab");
            //                System.Threading.Thread.Sleep(500);
            //            }

            //            await page.Keyboard.TypeAsync(schedule.Caption, new TypeOptions {Delay = 200});
            //            await page.EvaluateExpressionAsync(AutomaticPost.ClickButton("Share"));
            //            System.Threading.Thread.Sleep(10000);
            //            schedule.PostedStatus = true;
            //            schedule.LastTryDate = DateTime.Now;
            //            schedule.Exception = "";
            //        }
            //        catch (Exception ex)
            //        {
            //            schedule.PostedStatus = false;
            //            schedule.Exception = ex.ToString();
            //            schedule.LastTryDate = DateTime.Now;
            //        }

            //        await ScheduleContext.SaveChangesAsync();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.ToString() + ". " + k);
            //}
        }


        [HttpPost]

        public bool DeleteSchedule(int scheduleId)
        {
            var schedule = ScheduleContext.Schedules.FirstOrDefault(x => x.Id == scheduleId);
            if (schedule != null)
            {
                if (!string.IsNullOrWhiteSpace(schedule.JobId))
                {
                    BackgroundJob.Delete(jobId: schedule.JobId);
                }

                ScheduleContext.Schedules.Remove(schedule);
                ScheduleContext.SaveChanges();
            }
            Notify("Success", "Successfully Deleted", "User Deleted Successfully", IsRedirectMessage: true);
            return true;
        }
    }



    public class InstagramCredential
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public static class AutomaticPost
    {
        public  static ScheduleContext ScheduleContext = new ScheduleContext();
       
        public static string ClickButton(string btnName, bool disabled = false)
        {
            string disableScript = "";
            if (disabled)
            {
                disableScript = "btnList[i].disabled = false;";
            }
            string loginBtnScript = $@"
var btnList = document.getElementsByTagName(""button""); 
for (var i = 0; i < btnList.length; i++)
{{
    console.log(""loop working"");
    if (btnList[i].innerText == ""{btnName}"")
    {{
        {disableScript}
        btnList[i].click();
    }}
}}
                    ";
            return loginBtnScript;
        }

        public static async Task AutomaticPosts(int scheduleId = 0)
        {
            
            string k = string.Empty;
            try
            {
                var DateFrom = DateTime.Now.AddMinutes(-30);
                var DateTo = DateTime.Now;
                var schedules = new List<Schedule>();
                
                if (scheduleId != 0)
                {
                    schedules =  ScheduleContext.Schedules.Include(x=>x.User).Include(x=>x.User.Proxy).Where(x => x.Id == scheduleId).ToList();
                }
                else
                {
                    schedules = ScheduleContext.Schedules.Include(x => x.User).Include(x => x.User.Proxy).Where(x =>
                            x.PostedStatus == false && x.TimeStamp > DateFrom && x.TimeStamp <= DateTo)
                        .ToList();
                }

                string Url = "https://www.instagram.com/";

                var browerFetcher = new BrowserFetcher();
                await browerFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);
                foreach (var schedule in schedules)
                { 
                    bool proxy = schedule.User.Proxy != null;
                    using (var browser = Puppeteer.LaunchAsync(new LaunchOptions
                    {
                        
                        Args = proxy ? new string[1] { $"--proxy-server={schedule.User.Proxy.IpAddress}" } : new string[0] { },
                        ExecutablePath = browerFetcher.GetExecutablePath(BrowserFetcher.DefaultRevision),
                        Headless = false,
                        Timeout = 120000
                    }).GetAwaiter().GetResult())
                    {
                        var devices = Puppeteer.Devices;
                        var iphone = devices[DeviceDescriptorName.IPhone6];
                        var page = browser.NewPageAsync().GetAwaiter().GetResult();
                        await page.EmulateAsync(iphone);

                        try
                        {
                            await page.GoToAsync(Url);
                            string html = await page.GetContentAsync();
                            System.Threading.Thread.Sleep(5000);
                            await page.EvaluateExpressionAsync(ClickButton("Log In"));

                            System.Threading.Thread.Sleep(5000);
                            for (int i = 0; i < 4; i++)
                            {
                                await page.Keyboard.DownAsync(key: "Tab");
                                System.Threading.Thread.Sleep(500);
                            }
                            await page.Keyboard.TypeAsync(schedule.User.InstagramUsername, new TypeOptions { Delay = 200 });
                            await page.Keyboard.DownAsync(key: "Tab");
                            await page.Keyboard.TypeAsync(schedule.User.InstagramPassword, new TypeOptions { Delay = 200 });
                        
                            await page.EvaluateExpressionAsync(ClickButton("Log In", true));
                            System.Threading.Thread.Sleep(5000);
                            await page.WaitForNavigationAsync();

                            await page.EvaluateExpressionAsync(ClickButton("Save Info"));
                            System.Threading.Thread.Sleep(5000);
                            await page.WaitForNavigationAsync();
                            System.Threading.Thread.Sleep(2000);
                            await page.EvaluateExpressionAsync(ClickButton("Cancel"));
                            page.EvaluateExpressionAsync(@"document.querySelector(""div[data-testid=new-post-button]"").click()");
                            var fileChooser = await page.WaitForFileChooserAsync();
                            await fileChooser.AcceptAsync(new string[] { HostingEnvironment.MapPath(schedule.ImagePath) });
                            System.Threading.Thread.Sleep(5000);

                            await page.EvaluateExpressionAsync(ClickButton("Next"));
                            System.Threading.Thread.Sleep(5000);
                            await page.WaitForNavigationAsync();

                            for (int i = 0; i < 4; i++)
                            {
                                await page.Keyboard.DownAsync(key: "Tab");
                                System.Threading.Thread.Sleep(500);
                            }
                            await page.Keyboard.TypeAsync(schedule.Caption, new TypeOptions { Delay = 200 });

                            await page.EvaluateExpressionAsync(ClickButton("Share"));
                            System.Threading.Thread.Sleep(5000);
                            await page.WaitForNavigationAsync();

                            schedule.PostedStatus = true;
                            schedule.LastTryDate = DateTime.Now;
                            schedule.Exception = "";

                        }
                        catch (Exception ex)
                        {
                            schedule.PostedStatus = false;
                            schedule.Exception = ex.ToString();
                            schedule.LastTryDate = DateTime.Now;
                        }
                        await browser.CloseAsync();
                        await browser.DisposeAsync();
                        browser.Disconnected += (sender, args) =>
                        {
                            try
                            {
                                foreach (var process in Process.GetProcessesByName("chrome"))
                                {
                                    process.Kill();
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                        };
                        await ScheduleContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + ". " + k);
            }


        }

    }


}
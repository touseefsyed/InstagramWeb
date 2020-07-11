using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Hangfire;
using InstagramWeb.Common;
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
            if (user.RoleId == (int)Roles.Admin)
            {
                scheduleList = ScheduleContext.Schedules.ToList();
            }
            else
            {
                scheduleList = ScheduleContext.Schedules.Where(x => x.UserId == user.Id).ToList();
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
                    schedule = ScheduleContext.Schedules.FirstOrDefault(x => x.Id == Id && x.UserId == user.Id);
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
            obj.TimeStamp = obj.TimeStamp.ToLocalTime();
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
                    string jobId = BackgroundJob.Schedule(() => AutomaticPost.AutomaticPosts(obj.Id, true), obj.TimeStamp);
                    obj.JobId = jobId;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(obj.JobId))
                    {
                        BackgroundJob.Delete(obj.JobId);
                        string jobId = BackgroundJob.Schedule(() => AutomaticPost.AutomaticPosts(obj.Id, true), obj.TimeStamp);
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
        public async Task<ActionResult> SchedulePost(int ScheduleId)
        {
            await Schedule(ScheduleId);
            return RedirectToAction("AddSchedule", "Schedule");
        }

        private async Task Schedule(int ScheduleId)
        {
            await AutomaticPost.AutomaticPosts(ScheduleId);
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
        public static ScheduleContext ScheduleContext = new ScheduleContext();

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


        public static async Task GetDailyFollowers()
        {
            ScheduleContext scheduleContext = new ScheduleContext();
            foreach (var item in scheduleContext.Users.ToList())
            {
                if (string.IsNullOrWhiteSpace(item.InstagramUsername))
                {
                    continue;
                }

                string Url = "https://www.instagram.com/";
                var browerFetcher = new BrowserFetcher();
                await browerFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);

                bool proxy = item.Proxy != null;
                using (var browser = Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Args = proxy ? new string[2] { $"--proxy-server={item.Proxy.IpAddress}", "--ignore-certificate-errors" } : new string[0] { },
                    ExecutablePath = browerFetcher.GetExecutablePath(BrowserFetcher.DefaultRevision),
                    Headless = false,
                    Timeout = 120000
                }).GetAwaiter().GetResult())
                {
                    var devices = Puppeteer.Devices;
                    var iphone = devices[DeviceDescriptorName.IPhone6];
                    var page = browser.NewPageAsync().GetAwaiter().GetResult();
                    await page.EmulateAsync(iphone);

                    long followerCount = 0;
                    string exception = string.Empty;
                    var ApiLink = $"https://www.instagram.com/{item.InstagramUsername}";
                    string k = string.Empty;
                    try
                    {
                        
                        if (item.Proxy != null && !string.IsNullOrWhiteSpace(item.Proxy.Username))
                        {
                            var credentials = new Credentials
                            {
                                Password = item.Proxy.Password,
                                Username = item.Proxy.Username
                            };
                            await page.AuthenticateAsync(credentials);
                        }

                        await page.GoToAsync(Url);
                        string html = await page.GetContentAsync();
                        System.Threading.Thread.Sleep(5000);
                        if (html.Contains("coreSpriteDismissLarge"))
                        {
                            await page.EvaluateExpressionAsync(ClickButton("Close"));
                            System.Threading.Thread.Sleep(2000);
                        }
                        await page.EvaluateExpressionAsync(ClickButton("Log In"));


                        System.Threading.Thread.Sleep(5000);
                        for (int i = 0; i < 4; i++)
                        {
                            await page.Keyboard.DownAsync(key: "Tab");
                            System.Threading.Thread.Sleep(500);
                        }
                        await page.Keyboard.TypeAsync(item.InstagramUsername, new TypeOptions { Delay = 200 });
                        await page.Keyboard.DownAsync(key: "Tab");
                        await page.Keyboard.TypeAsync(item.InstagramPassword, new TypeOptions { Delay = 200 });

                        await page.EvaluateExpressionAsync(ClickButton("Log In", true));
                        System.Threading.Thread.Sleep(500);
                        await PageNavigation(page);
                        System.Threading.Thread.Sleep(5000);
                        await page.GoToAsync(ApiLink);
                        await PageNavigation(page);
                        k = await page.GetContentAsync();
                        //"userInteractionCount":"2836"
                        string l = k.Split(new string[] { "userInteractionCount" }, StringSplitOptions.None)[1];
                        string m = l.Split(new string[] { ":" }, StringSplitOptions.None)[1];
                        var count = m.Split('}').Where(x => !string.IsNullOrEmpty(x)).FirstOrDefault();
                        count = count.Replace("\"", "").Replace(",", "");
                   
                        followerCount = long.Parse(count);
                    }
                    catch (Exception ex) 
                    {
                        exception = ex.ToString() + ", Url = " + ApiLink +", Html = "+k;
                    }

                    var existingRecord = scheduleContext.DailyFollowerCount
                        .FirstOrDefault(x => x.UserId == item.Id 
                        && x.RecordedDate.Year == DateTime.Now.Year 
                        && x.RecordedDate.Month == DateTime.Now.Month 
                        && x.RecordedDate.Day == DateTime.Now.Day );
                    if (existingRecord != null)
                    {
                        existingRecord.Followers = followerCount;
                        existingRecord.RecordedDate = DateTime.Now;
                        existingRecord.UserId = item.Id;
                        existingRecord.Exception = exception;
                        scheduleContext.SaveChanges();
                    }
                    else 
                    {
                        scheduleContext.DailyFollowerCount.Add(new DailyFollowerCount
                        {
                            Followers = followerCount,
                            RecordedDate = DateTime.Now,
                            UserId = item.Id,
                            Exception = exception
                        });
                        scheduleContext.SaveChanges();
                    }
                }

                //var instagramApiLink = $"https://www.instagram.com/{item.InstagramUsername}";
                //using (var client = new HttpClient())
                //{
                //    string k = string.Empty;
                //    int followerCount = 0;
                //    string exception = string.Empty;
                //    try
                //    {
                //        var result = client.GetAsync(instagramApiLink).GetAwaiter().GetResult();
                //        k = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                //        string l = k.Split(new string[] { "Followers" }, StringSplitOptions.None)[0];
                //        var count = l.Split(' ').Where(x => !string.IsNullOrEmpty(x)).LastOrDefault();
                //        count = count.Replace("\"", "").Replace(",", "").Split('=')[1];
                //        followerCount = int.Parse(count);
                //    }

                //    catch (Exception ex)
                //    {
                //        exception = ex.ToString() + ": APILink = " + instagramApiLink + ",Html : " + k;
                //    }


                //    scheduleContext.DailyFollowerCount.Add(new DailyFollowerCount
                //    {
                //        Followers = followerCount,
                //        RecordedDate = DateTime.Now,
                //        UserId = item.Id,
                //        Exception = exception
                //    });
                //    scheduleContext.SaveChanges();
                //}
            }
        }

        public static async Task AutomaticPosts(int scheduleId = 0, bool retry = true)
        {
            string k = string.Empty;
            try
            {
                var DateFrom = DateTime.Now.AddMinutes(-30);
                var DateTo = DateTime.Now;
                var schedules = new List<Schedule>();

                if (scheduleId != 0)
                {
                    schedules = ScheduleContext.Schedules.Include(x => x.User).Include(x => x.User.Proxy).Where(x => x.Id == scheduleId).ToList();
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
                        Args = proxy ? new string[2] { $"--proxy-server={schedule.User.Proxy.IpAddress}", "--ignore-certificate-errors" } : new string[0] { },
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
                            if (schedule.User.Proxy != null && !string.IsNullOrWhiteSpace(schedule.User.Proxy.Username))
                            {
                                var credentials = new Credentials
                                {
                                    Password = schedule.User.Proxy.Password,
                                    Username = schedule.User.Proxy.Username
                                };
                                await page.AuthenticateAsync(credentials);
                            }

                            await page.GoToAsync(Url);
                            string html = await page.GetContentAsync();
                            System.Threading.Thread.Sleep(5000);
                            if (html.Contains("coreSpriteDismissLarge"))
                            {
                                await page.EvaluateExpressionAsync(ClickButton("Close"));
                                System.Threading.Thread.Sleep(2000);
                            }

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
                            System.Threading.Thread.Sleep(500);
                            await PageNavigation(page);
                            System.Threading.Thread.Sleep(5000);

                            await page.EvaluateExpressionAsync(ClickButton("Save Info"));
                            System.Threading.Thread.Sleep(500);
                            await PageNavigation(page);
                            System.Threading.Thread.Sleep(5000);

                            await page.EvaluateExpressionAsync(ClickButton("Cancel"));
                            System.Threading.Thread.Sleep(2000);

                            if (schedule.Type.ToLower() == "story")
                            {
                                var html2 = await page.GetContentAsync();
                                if (html2.Contains("Welcome to Instagram"))
                                {
                                    page.EvaluateExpressionAsync(@"document.querySelector(""header button"").click()");
                                    var fileChooser = await page.WaitForFileChooserAsync();
                                    await fileChooser.AcceptAsync(new string[] { HostingEnvironment.MapPath(schedule.ImagePath) });
                                    await PageNavigation(page);
                                }
                                else
                                {
                                    try
                                    {
                                        page.EvaluateExpressionAsync(ClickButton("Your Story"));
                                        var fileChooser = await page.WaitForFileChooserAsync();
                                        await fileChooser.AcceptAsync(new string[] { HostingEnvironment.MapPath(schedule.ImagePath) });
                                        await PageNavigation(page);
                                    }
                                    catch (Exception ex)
                                    {
                                        page.EvaluateExpressionAsync(ClickButton("Not Now"));
                                        var fileChooser = await page.WaitForFileChooserAsync();
                                        await fileChooser.AcceptAsync(new string[] { HostingEnvironment.MapPath(schedule.ImagePath) });
                                        await PageNavigation(page);
                                    }
                                }


                                try
                                {

                                    System.Threading.Thread.Sleep(5000);
                                    await page.EvaluateExpressionAsync(ClickButton("Add to your story"));
                                    System.Threading.Thread.Sleep(500);
                                    await PageNavigation(page);
                                    System.Threading.Thread.Sleep(5000);
                                    schedule.PostedStatus = true;
                                    schedule.LastTryDate = DateTime.Now;
                                    schedule.Exception = "";

                                }
                                catch (Exception ex)
                                {
                                    schedule.PostedStatus = true;
                                    schedule.LastTryDate = DateTime.Now;
                                    schedule.Exception = "Story Already Posted";
                                }
                            }
                            else
                            {

                                page.EvaluateExpressionAsync(@"document.querySelector(""div[data-testid=new-post-button]"").click()");
                                var fileChooser = await page.WaitForFileChooserAsync();
                                await fileChooser.AcceptAsync(new string[] { HostingEnvironment.MapPath(schedule.ImagePath) });
                                await PageNavigation(page);
                                System.Threading.Thread.Sleep(5000);

                                await page.EvaluateExpressionAsync(ClickButton("Next"));
                                await PageNavigation(page);

                                System.Threading.Thread.Sleep(5000);

                                for (int i = 0; i < 4; i++)
                                {
                                    await page.Keyboard.DownAsync(key: "Tab");
                                    System.Threading.Thread.Sleep(500);
                                }
                                await page.Keyboard.TypeAsync(schedule.Caption, new TypeOptions { Delay = 200 });

                                await page.EvaluateExpressionAsync(ClickButton("Share"));
                                System.Threading.Thread.Sleep(500);
                                await PageNavigation(page);
                                System.Threading.Thread.Sleep(5000);

                                schedule.PostedStatus = true;
                                schedule.LastTryDate = DateTime.Now;
                                schedule.Exception = "";
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                string filename = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".png";
                                string path = HostingEnvironment.MapPath("~/Posts/Screenshots/" + filename);
                                await page.ScreenshotAsync(path);
                            }
                            catch (Exception ex1)
                            {
                                schedule.Exception = ex1.ToString();
                            }

                            schedule.PostedStatus = false;
                            schedule.Exception += ex.ToString();
                            schedule.LastTryDate = DateTime.Now;
                            if (!string.IsNullOrWhiteSpace(schedule.Exception))
                            {
                                throw new Exception(schedule.Exception);
                            }
                        }
                        await browser.CloseAsync();
                        await browser.DisposeAsync();
                        browser.Disconnected += (sender, args) =>
                        {
#if !DEBUG
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
#endif

                        };
                        await ScheduleContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                if (retry)
                {
                    await AutomaticPosts(scheduleId, false);
                }

                throw new Exception(ex.ToString() + ". " + k);
            }


        }

        private static async Task PageNavigation(Page page)
        {
            try
            {
                await page.WaitForNavigationAsync();
            }
            catch (Exception ex)
            {
            }
        }
    }


}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Storage;
using InstagramWeb.Controllers;
using InstagramWeb.Models;

namespace InstagramWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            HangfireAspNet.Use(GetHangfireServers);
            ScheduleContext context = new ScheduleContext();
            //string UserName = "ash.barbour";
            //    string Password = "getfruitypost";
            if (context.Users.Count(x => x.Username == "ash.barbour") == 0)
            {
                var user = new User
                {
                    Username = "ash.barbour",
                    RoleId =  (int)Roles.Admin,
                    Email = "ash.barbour@getfruity.co",
                    Password = "getfruitypost",
                    FirstName = "Ash",
                    LastName = "Barbour",
                    EmailVerified = true,
                    InActive = false
                };
                context.Users.Add(user);
                context.SaveChanges();
            }

        }

        protected void Application_AuthenticateRequest()
        {

        }

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                });

            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }

            RecurringJob.AddOrUpdate(() => AutomaticPost.AutomaticPosts(0,false), Cron.Hourly);

            yield return new BackgroundJobServer();
        }

    }
}

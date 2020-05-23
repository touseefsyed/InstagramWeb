using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InstagramWeb.Models
{
    public class ScheduleContext : DbContext
    {
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleImage> ScheduleImages { get; set; }
        public DbSet<ScheduleVideo> ScheduleVideos { get; set; }


        public ScheduleContext() : base("name=DefaultConnectionString")
        {

        }

    }

}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace InstagramWeb.Models
{
    [Table("Schedule", Schema = "Schedules")]
    public class Schedule
    {

        [Key]
        public int ID { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<ScheduleImage> scheduleImages { get; set; }
        public List<ScheduleVideo> scheduleVideos { get; set; }

    }
    [Table("ScheduleImage", Schema = "Schedules")]
    public class ScheduleImage
    {
        [Key]
        public int ID { get; set; }
        public string imagePath { get; set; }

        public int ScheduleID { get; set; }
        [ForeignKey("ScheduleID")]
        public Schedule Schedule { get; set; }
    }
    [Table("ScheduleVideo", Schema = "Schedules")]
    public class ScheduleVideo
    {
        [Key]
        public int ID { get; set; }
        public string videoPath { get; set; }
        
        public int ScheduleID { get; set; }
        [ForeignKey("ScheduleID")]
        public Schedule Schedule { get; set; }

    }

}

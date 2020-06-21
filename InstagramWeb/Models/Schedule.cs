using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace InstagramWeb.Models
{
    [Table("Schedule", Schema = "Schedules")]
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public string Caption { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        [DisplayName("Schedule At")]
        public DateTime TimeStamp { get; set; }
        public bool? PostedStatus { get; set; }
        public DateTime? LastTryDate { get; set; }
        [AllowHtml]
        public string Exception { get; set; }
        public string Type { get; set; }
        public string JobId { get; set; }
        [DisplayName("User")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public  User User { get; set; }
    }

}

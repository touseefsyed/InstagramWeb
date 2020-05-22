using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstagramWeb.Models
{
    public class Schedule
    {
        public long ID { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
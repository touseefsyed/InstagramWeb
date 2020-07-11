using InstagramWeb.Models;
using System.Collections.Generic;

namespace InstagramWeb.Models.ViewModels
{
    public class UserExceptionViewModel
    {
        public List<User> Users { get; set; }
        public DailyFollowerCountFilter DailyFollowerCountFilter { get; set; }
        public List<DailyFollowerCount> DailyFollowerCount { get; set; }
    }
}
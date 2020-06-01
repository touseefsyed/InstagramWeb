using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstagramWeb.Models.ViewModels
{
    public class UserListViewModel
    {
        public User UserFilter { get; set; }
        public List<User> Users { get; set; }
    }
}
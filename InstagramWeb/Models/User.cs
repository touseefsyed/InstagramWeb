﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InstagramWeb.Models
{
    public enum Roles
    {
        Admin = 1,
        User = 2
    }


    [Table("User", Schema = "Accounts")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool EmailVerified { get; set; }
        public bool InActive { get; set; }

        [Required(ErrorMessage ="User Name is Required")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ImagePath { get; set; }

        public string InstagramUsername { get; set; }
        public string InstagramPassword { get; set; }

        public string InstagramUserId { get; set; }
        public string InstagramAuthToken { get; set; }
        public string FacebookAuthToken { get; set; }
        public string FacebookPageId { get; set; }


        public int? ProxyId { get; set; }
        [ForeignKey("ProxyId")]
        public Proxy Proxy { get; set; }

    }


    [Table("Proxy",Schema = "Accounts")]
    public class Proxy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [Required]
        public string IpAddress { get; set; }
        public int Timeouts { get; set; }
    }

    [Table("DailyFollowerCount", Schema = "Accounts")]
    public class DailyFollowerCount
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime RecordedDate { get; set; }
        public long Followers { get; set; }

        public string Exception { get; set; }
    

    }

    public class DailyFollowerCountFilter
    {
        public int? UserId { get; set; }
        public DateTime? RecordedDateFrom { get; set; }
        public DateTime? RecordedDateTo { get; set; }
        public int? Followers { get; set; }
        public string Exception { get; set; }
    }
}
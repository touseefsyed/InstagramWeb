using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstagramWeb.Models.ViewModels
{
    public class RegisterRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Username")]
        [Remote("IsAvailableUsername", "Validation", AdditionalFields = "Id")]
        public string Username { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [StringLength(50)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(250)]
        [Remote("IsAvailableEmail", "Validation", AdditionalFields = "Id")]
        public string Email { get; set; }
        [Required]
        [StringLength(200)]
        public string Password { get; set; }
        [Required]
        [StringLength(200)]
        [System.ComponentModel.DataAnnotations.Compare(("Password"))]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Register As")]
        public int RoleId { get; set; }
        
        public string ImagePath { get; set; }

        [Required]
        [DisplayName("Instagram Username(Not email or phone)")]
   
        public string InstagramUsername { get; set; }
        [Required]
        [DisplayName("Instagram Password")]
        public string InstagramPassword { get; set; }

        public int? ProxyId { get; set; }

        public List<Proxy> Proxies { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }

        public static User ToUser(RegisterRequest r)
        {
            var user = new User
            {
                Id = r.Id,
                Username = r.Username,
                FirstName = r.FirstName,
                LastName = r.LastName,
                Password = r.Password,
                Email = r.Email,
                RoleId = r.RoleId,
                ImagePath = r.ImagePath,
                ProxyId = r.ProxyId,
                InstagramPassword =  r.InstagramPassword,
                InstagramUsername = r.InstagramUsername

            };
            return user;
        }
    }

}
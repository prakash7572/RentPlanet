using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RentPlannet.Models
{
    public class Profile
    {
        public int ID { get; set; }
        public string UserName { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string Password { get; set;}
        public string Email { get; set;}
        public string Mobile { get; set;}
        public bool Remember { get; set;}
        public string Otp { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Description { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime CurrDate { get; set; }
        public string CoverImage { get; set; }
        public string ProfileImage { get; set; }
        public int? Overview { get; set; }
    }
    public class LoginVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
    public class ChangePassword
    {
        [Key]
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
    [Table("UserActiveDurations")]
    public class UserActiveDuration
    {
      [Key]
      public int UserId { get; set; }
      public int ProfileId { get; set; }
      public string LoginTime { get; set; }
      public string LogOutTime { get; set; }
      public string LoginDuration { get; set; }
      public string SumLoginDuration { get; set; }
      public DateTime CurrDate { get; set; }
    }

    


}

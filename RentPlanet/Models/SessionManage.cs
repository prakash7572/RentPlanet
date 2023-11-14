using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace RentPlannet.Models
{
    public static class SessionManage
    {
        public class SessionData
        {
            public static int? ID { get; set; }
            public static DateTime? CurrDate { get; set; }
            public static DateTime? LoginTime { get; set; }
            public static string UserName { get; set; } = null;
            public static string PhoneNumber { get; set; }=null;
            public static string Email { get; set; } = null;
            public static DateTime JoiningDate { get; set; }
            public static string City { get; set; } = null;
            public static string Country { get; set; } = null;
            public static string ZipCode { get; set; } = null;
            public static string Description { get; set; }  = null;
            public static string ProfileImage { get; set; } = null;
            public static string CoverImage { get; set; } = null;       
            public static int? Overview { get; set; }
            
        }
    }
}
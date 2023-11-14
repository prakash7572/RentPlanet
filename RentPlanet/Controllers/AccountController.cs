using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using RentPlannet.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using static RentPlannet.Models.SessionManage;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace RentPlannet.Controllers
{
   
    public class AccountController : Controller
    {
        Data_Connection _db = new Data_Connection();
        [HttpGet]
        public ActionResult SignUp()
        {
            return View("/views/account/signup.cshtml");
        }
        [HttpPost]
        public ActionResult SignUp(Profile profile)
        {
            try
            {
                _db.Profiles.Add(profile);
                _db.SaveChanges();
                return RedirectToAction("dashbord", "account");
            }
            catch (Exception ex)
            {
                return View("/views/shared/error.cshtml", ex);
            }
          
        }
        public ActionResult SignIn()
        {    
            return View("/views/account/signin.cshtml");
        }
        [HttpPost]
        public ActionResult SignInAuth(LoginVM model)
            {
            try
            {
                Profile profile = _db.Profiles.Where(x => x.UserName == model.UserName && x.Password == model.Password).FirstOrDefault();
                if (profile != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Password, false);
                    SessionData.LoginTime = DateTime.Now;
                    SessionData.CurrDate = profile.CurrDate;
                    SessionData.ID = profile.ID;
                    SessionData.UserName = profile.UserName;
                    SessionData.PhoneNumber = profile.Mobile;
                    SessionData.Email = profile.Email;
                    SessionData.JoiningDate = profile.JoiningDate;
                    SessionData.City = profile.City;
                    SessionData.Country = profile.Country;
                    SessionData.ZipCode = profile.ZipCode;
                    SessionData.Description = profile.Description;
                    SessionData.ProfileImage = profile.ProfileImage;
                    SessionData.CoverImage = profile.CoverImage;
                    SessionData.Overview = profile.Overview;
                    return RedirectToAction("Dashbord");
                }
                else
                {
                    TempData["ERROR"] = "Invalid UserName & Password **";
                    return View("/views/account/signin.cshtml");
                }
            }
            catch (Exception ex)
            {

                return View("/views/shared/error.cshtml", ex);

            }

        }
        [System.Web.Mvc.Authorize]
        public ActionResult SignOut(UserActiveDuration user,Profile profile)
        {
            try
            {

                FormsAuthentication.SignOut();
                //DateTime loginTime = (DateTime)Session["LoginTime"];
                DateTime loginTime = Convert.ToDateTime(SessionManage.SessionData.LoginTime);
                TimeSpan duration = DateTime.Now - loginTime;
                string formattedDuration = $"{duration.Hours}:{duration.Minutes}:{duration.Seconds}";
                //int id = (int)Session["UserId"];
                int id = Convert.ToInt16(SessionManage.SessionData.ID);
                user.ProfileId = id;
                user.LoginTime = Convert.ToString(loginTime);
                user.LogOutTime = Convert.ToString(DateTime.Now);
                user.LoginDuration = formattedDuration;
                _db.UserActiveDurations.Add(user);
                _db.SaveChanges();
                var totalhoures = _db.UserActiveDurations.Where(x => x.UserId == x.UserId).AsEnumerable()
               .Select(e => TimeSpan.Parse(e.LoginDuration))
               .Aggregate(TimeSpan.FromMinutes(0), (total, next) => total + next).ToString();
                user.SumLoginDuration = totalhoures;
                _db.SaveChanges();
                return View("/views/rentplanet/dashbord.cshtml");

            }
            catch (Exception ex)
            {
                return View("/views/shared/error.cshtml", ex);

            }
        }
        public ActionResult ForgetPassword()
        {
            return View("/views/account/forgetpassword.cshtml");
        }
        [AllowAnonymous]
        public ActionResult Dashbord(Profile profile)
        {
            try
            {
                ViewBag.AcivePerson =  GenerateOTP();
                ViewBag.TotalOnlineUsers = Session["TotalOnlineUsers"];
                DateTime currentDate = DateTime.Now.Date;
                int currentMonth = currentDate.Month;
                var lastProfileId = _db.UserActiveDurations
                    .OrderByDescending(data => data.UserId)
                    .Select(data => data.SumLoginDuration)
                    .FirstOrDefault();
                    ViewBag.hourthismonth = lastProfileId;
                var list = _db.Profiles.ToList();
                    ViewBag.Person = list.Count();
                var lists = _db.Profiles.GroupBy(u => u.CurrDate).Select(group => new{
                Month = group.Key.ToString(),
                UserCount = group.Count()}).ToList();
                foreach (var item in lists)
                {string month = item.Month;
                int userCount = item.UserCount;
                ViewBag.thisMonth = userCount;}
                string username = SessionManage.SessionData.UserName;
                string profileimage = SessionManage.SessionData.ProfileImage;
                ViewBag.UserName = username;
                ViewBag.ProfileImage = profileimage;
                //for Overview   
                int id = Convert.ToInt16(SessionManage.SessionData.ID);
                int overview = Convert.ToInt16(SessionManage.SessionData.Overview);
                Profile pro = _db.Profiles.Where(x => x.ID == id).FirstOrDefault();
                ViewBag.Overview = pro.Overview;
                pro.Overview = _db.Profiles.Max(x => x.Overview) + 1;
                _db.Entry(pro).State = EntityState.Modified;
                _db.SaveChanges();
                //end Overview
                return View("/views/account/dashbord.cshtml");
            }
            catch (Exception ex)
            {
                return View("/views/shared/error.cshtml",ex);
            }
           
        }
        [HttpPost]
        public ActionResult SendOtpEmail(string toEmail)
        {
            try
            {
                Profile profile = new Profile();
                if (!string.IsNullOrEmpty(toEmail))
                {
                    profile = _db.Profiles.FirstOrDefault(x => x.Email == toEmail);
                    if (profile.Email.Equals(toEmail))
                    {
                        string otp = GenerateOTP();
                        profile.Otp = otp;
                        _db.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                        if(_db.SaveChanges() > 0)
                        {
                            MailMessage message = new MailMessage();
                            message.From = new MailAddress("praksh5785@gmail.com"); // Sender email address
                            message.To.Add(new MailAddress(toEmail));
                            message.Subject = "One-Time Password (OTP)";
                            message.Body = $"Your OTP is: {otp}";
                            SmtpClient smtpClient = new SmtpClient();
                            smtpClient.Host = ConfigurationManager.AppSettings["SmtpHost"];
                            smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                            smtpClient.EnableSsl = true;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = new NetworkCredential(
                                ConfigurationManager.AppSettings["SmtpUsername"],
                                ConfigurationManager.AppSettings["SmtpPassword"]
                            );
                            smtpClient.Send(message);
                            ViewBag.Message = "OTP sent successfully!";
                            return RedirectToAction("VerifyOtp");
                        }  
                    }
                }
                else 
                {
                    ViewBag.Message = "User not found.";
                    return View(toEmail);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
                return View("ForgetPassword");
            }
            return RedirectToAction("ForgetPassword");

        }
        private string GenerateOTP()
        {
            int otpLength = 6; 
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, otpLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public ActionResult VerifyOtp()
        {
            return View("/views/account/verifyotp.cshtml");

        }
        [HttpPost]
        public ActionResult VerifyOtp(string Otp)
        {
            try
            {
                var otpEntity = _db.Profiles.FirstOrDefault(x => x.Otp == Otp);
                if (otpEntity == null)
                {
                    ViewBag.Message = "Invalid email or OTP.";
                    return RedirectToAction("VerifyOtp"); 
                }
                ViewBag.Message = "OTP verified successfully!";
                 otpEntity.Otp = null;
                _db.SaveChanges();            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
                return View("/views/shared/error.cshtml", ex);
            }
            return View("/views/rentplanet/dashbord.cshtml");
        }
        [System.Web.Mvc.Authorize]
        public ActionResult Profiles()
        {
            try
            {
                ViewBag.Id = Convert.ToInt16(SessionManage.SessionData.ID);
                string username = SessionManage.SessionData.UserName;
                string[] parts = username.Split(new char[] { ' ' }, 2);
                if (parts.Length >= 2)
                {
                    string firstName = parts[0];
                    string lastName = parts[1];
                    ViewBag.FirstName = firstName;
                    ViewBag.LastName = lastName;
                }
                else if (parts.Length == 1)
                {
                    string firstName = parts[0];
                    ViewBag.FirstName = firstName;
                }
                ViewBag.Phonenumber = SessionManage.SessionData.PhoneNumber;
                ViewBag.Email = SessionManage.SessionData.Email;
                DateTime date = Convert.ToDateTime(SessionManage.SessionData.JoiningDate);
                ViewBag.Date = date.ToString("yyyy-MM-dd");
                ViewBag.City  = SessionManage.SessionData.City;
                ViewBag.Country = SessionManage.SessionData.Country;
                ViewBag.Zipcode = SessionManage.SessionData.ZipCode;
                ViewBag.Description = SessionManage.SessionData.Description;
                ViewBag.ProfileImage = SessionManage.SessionData.ProfileImage;
                ViewBag.CoverImage = SessionManage.SessionData.CoverImage;
                ViewBag.UserName = username;
                return View("/views/account/profile.cshtml");
            }
            catch (Exception ex)
            {
                return View("/views/shared/error.cshtml", ex);
            }
           
        }
        [HttpPost]
        public ActionResult ChangePassword(RentPlannet.Models.ChangePassword model)
        { 
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _db.Profiles.Where(x => x.Password == model.OldPassword).FirstOrDefault();

                    if (user != null && model.NewPassword == model.ConfirmPassword)
                    {
                        user.Password = model.NewPassword;
                        _db.Profiles.Attach(user);
                        _db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                        return RedirectToAction("SignIn");
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View("/views/account/signup.cshtml",ex);
            }
        }
        [HttpPost]
        public ActionResult UpdatePersonalDetails(Profile profile,HttpPostedFileBase ProfileImage, HttpPostedFileBase CoverImage)
        {
            var profil = _db.Profiles.Where(x=>x.ID==profile.ID).FirstOrDefault();
            if (ProfileImage != null)
            {
                profil.ProfileImage = ProfileImage.FileName;
                string path = Server.MapPath("~/Assests/assets/images/users/" + ProfileImage.FileName);
                ProfileImage.SaveAs(path);
            }
            if (CoverImage != null)
            {
                profil.CoverImage = CoverImage.FileName;
                string path = Server.MapPath("~/Assests/assets/images/users/" + CoverImage.FileName);
                CoverImage.SaveAs(path);
            }
            profil.UserName = profile.FirstName +" "+ profile.LastName;
            profil.FirstName = profile.FirstName;
            profil.JoiningDate = profile.JoiningDate;
            profil.LastName = profile.LastName;
            profil.Email = profile.Email;
            profil.Mobile = profile.Mobile;
            profil.Country = profile.Country;
            profil.City = profile.City;
            profil.Description = profile.Description;
            profil.ZipCode = profile.ZipCode;
            _db.Profiles.Attach(profil);
            _db.Entry(profil).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Profiles");
        }
        
    }

}
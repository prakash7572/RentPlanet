using RentPlannet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentPlannet.Controllers
{
    public class ManageRoomController : Controller
    {
        Data_Connection _db = new Data_Connection();
        public ActionResult Room()
        {
            try
            {
                SessionData();
                List<ManageRoom> data = _db.ManageRooms.ToList();
                return View("/views/rentplanet/rooms.cshtml", data);

            }
            catch (Exception ex)
            {
                return View("/views/shared/error.cshtml", ex);
            }
        }
        public ActionResult ManageRooms(string City)
        {
            try
            {
                SessionData();
                var data = _db.ManageRooms.Where(x=>x.City==City || City == null).ToList();
                return View("/views/rentplanet/managerooms.cshtml", data);
            }
            catch (Exception ex)
            {
                return View("/views/shared/error.cshtml", ex);
            }
            
        }
        [HttpPost]
        public ActionResult ManageRooms(ManageRoom manage,HttpPostedFileBase[] file, HttpPostedFileBase[] File2, HttpPostedFileBase[] File3)
        {
            try
            {
                //if(File1 != null)
                //{
                //    manage.Image= File1.FileName;
                //    string path = Server.MapPath("~/Assests/assests/images" + File1.FileName);
                //    File1.SaveAs(path);   
                //}
                //else if (File2 != null)
                //{
                //    manage.Image = File2.FileName;
                //    string path = Server.MapPath("~/Assests/assests/images" + File1.FileName);
                //    File1.SaveAs(path);
                //}
                //else if (File3 != null)
                //{
                //    manage.Image = File3.FileName;
                //    string path = Server.MapPath("~/Assests/assests/images" + File1.FileName);
                //    File1.SaveAs(path);
                //}
                _db.ManageRooms.Add(manage);
                manage.ID = Convert.ToInt16(SessionManage.SessionData.ID);
                _db.SaveChanges();
                return Json(JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View("/views/shared/error.cshtml", ex);
            }
            
        }
        [HttpGet]
        public ActionResult Getid(int id)
        {
            try
            {
                var data = _db.ManageRooms.FirstOrDefault(x => x.RoomId == id);
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return View("/views/shared/error.cshtml", ex);
            }
            
        }   
        [HttpPost]
        public ActionResult UpdateRooms(ManageRoom room)
        {
            try
            {
                var obj = _db.ManageRooms.FirstOrDefault(x => x.ID == room.ID);
                obj.Country = room.Country;
                obj.State = room.State;
                obj.RoomLocation = room.RoomLocation;
                obj.PostelCode = room.PostelCode;
                obj.Metro = room.Metro;
                obj.Shop = room.Shop;
                obj.Religion = room.Religion;
                obj.Hospital = room.Hospital;
                obj.Contact = room.Contact;
                obj.PricesRoom = room.PricesRoom;
                obj.Description = room.Description;

                _db.ManageRooms.Attach(obj);
                _db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return Json(JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) 
            { return View("/views/shared/error.cshtml", ex);  
            }
           
        }
        public ActionResult States() {
            try 
            {
                List<State> data = _db.States.ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex) { return View("/views/shared/error.cshtml", ex);  }

        } 
        public ActionResult Cities(int id)
        {
            try
            {
                List<City> data = _db.Cities.Where(x => x.state_id == id).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex) { return View("/views/shared/error.cshtml", ex);  }
        }
        private void SessionData()
        {    
            string username = SessionManage.SessionData.UserName;
            string profileimage = SessionManage.SessionData.ProfileImage;
            ViewBag.UserName = username;
            ViewBag.ProfileImage = profileimage;
        }
    }
}
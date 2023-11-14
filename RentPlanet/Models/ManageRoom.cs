using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentPlannet.Models
{
    public class ManageRoom
    {
       [Key]
       public int RoomId { get; set; }  
       public int ID { get; set; }
       public string Country { get; set; }
       public string State { get; set; }
       public string City { get; set; }
       public string RoomLocation { get; set; }
       public int Metro { get;set; }
       public int Shop { get;set; }
       public int Religion { get; set; }
       public int Hospital { get; set; }
       public string Contact { get; set; }
       public decimal PricesRoom { get; set; }
       public string Description { get; set; }
       public string Image { get; set; }
       public string File1 { get; set; }
       public string File2 { get; set; }
       public string File3 { get; set; }
       public string PostelCode { get; set; }
        
    }
    public class State
    {
       public int id { get; set; }
       public string name { get; set; }
       public int country_id { get; set; }
    }
    public class City
    {
        public int id { get; set; }
        public string city { get; set; }
        public int state_id { get; set;}
    }
}
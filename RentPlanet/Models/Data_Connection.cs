using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentPlannet.Models
{
    public class Data_Connection : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<UserActiveDuration> UserActiveDurations { get; set; }
        public DbSet<ManageRoom> ManageRooms { get; set; }
    }
}
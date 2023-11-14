using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
[assembly:OwinStartupAttribute(typeof(RentPlannet.Startup))]

namespace RentPlannet
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
    public partial class Startup
    {
        public void Configuration(IAppBuilder app) 
        {
            app.MapSignalR();
            //ConfigureAuth(app);
        }
    }
}

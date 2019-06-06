using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Price_Grabber
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //Application Global Config Settings

           
        }

        protected void Session_Start()
        {
            //Application Global Config Settings
            Session["ApplicationName"] = "Price Grabber";
        }

        protected void Session_End()
        {

        }

        protected void Application_End()
        {

        }
    }
}

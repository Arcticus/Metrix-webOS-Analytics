using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Metrix
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.as[pmh]x(/.*)?" });

            routes.MapRoute(
               "Admin", // Route name
               "Admin/{action}/{id}", // URL with parameters
               new { controller = "Admin", action = "Index", id = UrlParameter.Optional } // Parameter defaults
           );

            routes.MapRoute(
               "Apps", // Route name
               "Apps/{action}/{packageid}", // URL with parameters
               new { controller = "Apps", action = "List", packageid = UrlParameter.Optional } // Parameter defaults
           );

            routes.MapRoute(
                  "Metrix",                                           // Route name
                  "Metrix/{packageid}",                            // URL with parameters
                  new { controller = "Metrix", action = "Index" }  // Parameter defaults
              );

            routes.MapRoute(
               "Home", // Route name
               "{action}", // URL with parameters
               new { controller = "Home", action = "Index"} // Parameter defaults
           );
           
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }
}
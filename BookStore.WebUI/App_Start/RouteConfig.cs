using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute( //url/
                null,
                "",
                new
                { controller = "Book",
                  action = "List",
                    specialization = (string)null,
                  pageno=1
                }
            );

            routes.MapRoute( //url/BookListPage2
                null,
                "BookListPage{pageno}",
                new
                {
                    controller = "Book",
                    action = "List",
                    specialization = (string)null
                }
            );

            routes.MapRoute(//url/التخصصIS
                null,
                "{specialization}",
                new
                {
                    controller = "Book",
                    action = "List",
                    pageno =1
                }
            );

            routes.MapRoute(//url/IS/Page2
                null,
                "{specialization}/Page{pageno}",
                new
                {
                    controller = "Book",
                    action = "List"
                },
                new
                {
                    pageno =@"\d+"
                }
            );

            //routes.MapRoute(
            //    name: null,
            //    url: "BookListPage{pageno}",
            //    defaults: new { controller = "Book", action = "List" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Book", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}

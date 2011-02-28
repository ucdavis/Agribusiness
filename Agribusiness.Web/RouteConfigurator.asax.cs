﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Agribusiness.Web
{
    public static class RouteConfigurator
    {
        public static void RegisterRoutes()
        {
            RouteCollection routes = RouteTable.Routes;
            routes.Clear();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }  // Parameter defaults
                );
        }
    }
}
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Agribusiness.Web
{
    public static class RouteConfigurator
    {
        // take a look at this
        // http://hanssens.com/2009/asp-net-mvc-subdomain-routing/

        public static void RegisterRoutes()
        {
            RouteCollection routes = RouteTable.Routes;
            routes.Clear();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                name: "acct",
                url: "Account/{action}",
                defaults: new { site = "", controller = "Account", action = "Index" }
                );

            //routes.Add(new SubdomainRoute());

            //routes.MapRoute(
            //    name: "agexec",
            //    url: "agexec.caesdo.caes.ucdavis.edu/{site}/{controller}/{action}/{id}",
            //    defaults: new { site = "agexec", controller = "Home", action = "Index" }
            //    );

            //routes.Add("agexec", new DomainRoute("agexec.caesdo.caes.ucdavis.edu", "{site}/{controller}/{action}/{id}", new { site="agexec", controller="Home", action="Seminar", Id=""}));

            routes.MapRoute(
                name: "sitebase",
                url: "{site}/{controller}/{action}/{id}",
                defaults: new { site = "", controller = "Home", action = "Index", id = UrlParameter.Optional }
                );

        }
    }

    public class SubdomainRoute : RouteBase
    {
        private const string Agexec = "agexec";
        private const string Agleadership = "agleadership";

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData returnvalue = null;

            var url = httpContext.Request.Headers["HOST"];
            var index = url.IndexOf(".");

            if (index < 0)
            {
                return returnvalue;
            }

            var subdomain = url.Substring(0, index);

            switch (subdomain.ToLower())
            {
                case Agexec:
                    returnvalue = new RouteData(this, new MvcRouteHandler());
                    returnvalue.Values.Add("site", Agexec);
                    returnvalue.Values.Add("controller", "Home");
                    returnvalue.Values.Add("action", "Index");
                    break;
                case Agleadership:
                    returnvalue = new RouteData(this, new MvcRouteHandler());
                    returnvalue.Values.Add("site", Agleadership);
                    returnvalue.Values.Add("controller", "Home");
                    returnvalue.Values.Add("action", "Index");
                    break;
                default:
                    break;
            }

            return returnvalue;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}
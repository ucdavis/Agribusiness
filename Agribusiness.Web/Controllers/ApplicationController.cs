using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Repositories;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.Web.Services;
using Microsoft.Practices.ServiceLocation;
using UCDArch.Web.Attributes;
using UCDArch.Web.Controller;

namespace Agribusiness.Web.Controllers
{
    [Version(MajorVersion = 2)]
    //[CatbertMessages]
    //[ServiceMessage("Agribusiness", ...)]
    public abstract class ApplicationController : SuperController
    {
        public string ErrorMessages
        {
            get { return (string)TempData[StaticIndexes.Key_ErrorMessage]; }
            set { TempData[StaticIndexes.Key_ErrorMessage] = value; }
        }

        public string Site { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sites = SiteService.LoadSiteDomains();

            // if we're using a valid subdomain, inject the site token
            var host = HttpContext.Request.Headers["HOST"];
            var index = host.IndexOf(".");
            if (index > 0)
            {
                var subdomain = host.Substring(0, index);
                var site = sites.FirstOrDefault(a => a.Value == subdomain);
                if (!string.IsNullOrEmpty(site.Key))
                {
                    filterContext.RouteData.Values["site"] = site.Key;    
                }
            }

            Site = filterContext.RouteData.Values["site"] as string;
            ViewData["site"] = Site;

            base.OnActionExecuting(filterContext);
        }

        public IRepositoryFactory RepositoryFactory = ServiceLocator.Current.GetInstance<IRepositoryFactory>();
    }
}

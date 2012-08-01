using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Repositories;
using Agribusiness.Web.App_GlobalResources;
using Microsoft.Practices.ServiceLocation;
using UCDArch.Web.Attributes;
using UCDArch.Web.Controller;

namespace Agribusiness.Web.Controllers
{
    [Version]
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

        public string[] Sites = new string[] { "agexec", "aglead" };

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var host = HttpContext.Request.Headers["HOST"];
            var index = host.IndexOf(".");
            if (index > 0)
            {
                var subdomain = host.Substring(0, index);
                if (Sites.Contains(subdomain))
                {
                    filterContext.RouteData.Values["site"] = subdomain;
                }
            }

            Site = filterContext.RouteData.Values["site"] as string;

            ViewData["site"] = Site;

            base.OnActionExecuting(filterContext);
        }

        public IRepositoryFactory RepositoryFactory = ServiceLocator.Current.GetInstance<IRepositoryFactory>();
    }
}

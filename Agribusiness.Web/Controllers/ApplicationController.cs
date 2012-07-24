using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
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
                    Site = subdomain;
                }
                else
                {
                    Site = filterContext.RouteData.Values["site"] as string;    
                }
            }
            else
            {
                Site = "wawa...";
            }
            
            //var index = host.IndexOf(".");
            //if (index > 0)
            //{
            //    var subdomain = host.Substring(0, index);

            //    if (Sites.Contains(subdomain))
            //    {
            //        Site = subdomain;    
            //    }
            //    else
            //    {
            //        Site = filterContext.RouteData.Values["site"] as string;    
            //    }
            //}
            //else
            //{
            //    Site = filterContext.RouteData.Values["site"] as string;    
            //}
            
            base.OnActionExecuting(filterContext);
        }

        //public Site LoadSite()
        //{
        //    var site = (Site)System.Web.HttpContext.Current.Cache[Site];

        //    if (site == null && !string.IsNullOrEmpty(Site))
        //    {
        //        site = Repository.OfType<Site>().Queryable.FirstOrDefault(a => a.Id == Site);
        //        if (site != null)
        //        {
        //            System.Web.HttpContext.Current.Cache[Site] = site;                    
        //        }
        //    }

        //    return site;
        //}

        //public void CacheSite(Site site)
        //{
        //    System.Web.HttpContext.Current.Cache[site.Id] = site;
        //}

        public IRepositoryFactory RepositoryFactory = ServiceLocator.Current.GetInstance<IRepositoryFactory>();
    }
}

using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Repositories;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Services;
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Site = filterContext.RouteData.Values["site"] as string;
            base.OnActionExecuting(filterContext);
        }

        public Site LoadSite()
        {
            var site = (Site)System.Web.HttpContext.Current.Cache[Site];

            if (site == null && !string.IsNullOrEmpty(Site))
            {
                site = Repository.OfType<Site>().Queryable.FirstOrDefault(a => a.Id == Site);
                if (site != null)
                {
                    System.Web.HttpContext.Current.Cache[Site] = site;                    
                }
            }

            return site;
        }

        public IRepositoryFactory RepositoryFactory = ServiceLocator.Current.GetInstance<IRepositoryFactory>();
    }
}

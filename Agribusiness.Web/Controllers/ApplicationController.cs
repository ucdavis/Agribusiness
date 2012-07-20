using System.Web.Mvc;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.Web.Controllers.Filters;
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

    }
}

using System.IO;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Site class
    /// </summary>
    public class SiteController : ApplicationController
    {
        //
        // GET: /Site/
        public ActionResult Index()
        {
            var siteList = RepositoryFactory.SiteRepository.Queryable;
            return View(siteList);
        }

        public ActionResult Details(string id)
        {
            var site = RepositoryFactory.SiteRepository.GetNullableById(id);
            if (site == null)
            {
                return RedirectToAction("Index");
            }

            return View(site);
        }

        public ActionResult Edit(string id)
        {
            var site = RepositoryFactory.SiteRepository.GetNullableById(id);
            if (site == null)
            {
                return RedirectToAction("Index");
            }
            
            return View(site);
        }

        [HttpPost]
        public ActionResult Edit(string id, string name, string description, HttpPostedFileBase logo, HttpPostedFileBase splash)
        {
            return View(new Site());
        }

        public FileResult GetLogo(string id)
        {
            var site = RepositoryFactory.SiteRepository.GetNullableById(id);
            if (site != null && site.Logo != null)
            {
                return File(site.Logo, site.LogoContentType);
            }

            return File(new byte[0], string.Empty);
        }

        public FileResult GetSplash(string id)
        {
            var site = RepositoryFactory.SiteRepository.GetNullableById(id);
            if (site != null && site.Logo != null)
            {
                return File(site.SplashImage, site.SplashContentType);
            }

            return File(new byte[0], string.Empty);
        }

    }
}

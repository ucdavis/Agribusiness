using System.IO;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Services;
using UCDArch.Web.Helpers;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Site class
    /// </summary>
    public class SiteController : ApplicationController
    {
        //
        // GET: /Site/
        [UserOnly]
        public ActionResult Index()
        {
            var siteList = RepositoryFactory.SiteRepository.Queryable;
            return View(siteList);
        }

        [UserOnly]
        public ActionResult Details(string id)
        {
            var site = RepositoryFactory.SiteRepository.GetNullableById(id);
            if (site == null)
            {
                return RedirectToAction("Index");
            }

            return View(site);
        }

        [UserOnly]
        public ActionResult Edit(string id)
        {
            var site = RepositoryFactory.SiteRepository.GetNullableById(id);
            if (site == null)
            {
                Message = "Site could not be found.";
                return RedirectToAction("Index");
            }
            
            return View(site);
        }

        [UserOnly]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(string id, string name, string description, string welcome, HttpPostedFileBase logo, HttpPostedFileBase splash)
        {
            var site = RepositoryFactory.SiteRepository.GetNullableById(id);
            
            if (site == null)
            {
                Message = "Site could not be found.";
                return RedirectToAction("Index");
            }

            site.Name = name;
            site.Description = description;
            site.Welcome = welcome;

            if (logo != null && logo.ContentLength > 0)
            {
                var ms = new MemoryStream();
                logo.InputStream.CopyTo(ms);
                site.Logo = ms.ToArray();
                site.LogoContentType = logo.ContentType;
            }

            if (splash != null && splash.ContentLength > 0)
            {
                var ms = new MemoryStream();
                splash.InputStream.CopyTo(ms);
                site.SplashImage = ms.ToArray();
                site.SplashContentType = splash.ContentType;
            }

            site.TransferValidationMessagesTo(ModelState);

            if (site.IsValid())
            {
                RepositoryFactory.SiteRepository.EnsurePersistent(site);
                Message = "Site has been saved.";
                SiteService.CacheSite(site);
                return RedirectToAction("Index");
            }

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
            if (site != null && site.SplashImage != null)
            {
                return File(site.SplashImage, site.SplashContentType);
            }

            return File(new byte[0], string.Empty);
        }

    }

    public class SitePostModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase Logo { get; set; }
        public HttpPostedFileBase Splash { get; set; }
    }
}

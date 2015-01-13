using System.Web.Mvc;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Admin class
    /// </summary>
    [UserOnly]
    public class AdminController : ApplicationController
    {
        public ActionResult People(int? seminarId)
        {
            var viewModel = AdminPeopleViewModel.Create(RepositoryFactory, SiteService.LoadSite(Site), seminarId);
            return View(viewModel);
        }
    }
}

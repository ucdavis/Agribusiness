using System;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using System.Web;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the SeminarApplication class
    /// </summary>
    public class SeminarApplicationController : ApplicationController
    {
        private readonly IRepository<Application> _applicationRepository;
        private readonly IFirmService _firmService;

        public SeminarApplicationController(IRepository<Application> applicationRepository, IFirmService firmService)
        {
            _applicationRepository = applicationRepository;
            _firmService = firmService;
        }

        [MembershipUserOnly]
        public ActionResult Index()
        {
            var applications = _applicationRepository.Queryable;
            return View(applications);
        }

        [MembershipUserOnly]
        public ActionResult Apply()
        {
            var viewModel = ApplicationViewModel.Create(Repository, _firmService, CurrentUser.Identity.Name);
            return View(viewModel);
        }

        [HttpPost]
        [MembershipUserOnly]
        public ActionResult Apply(Application application, HttpPostedFileBase file)
        {
            var viewModel = ApplicationViewModel.Create(Repository, _firmService, CurrentUser.Identity.Name, application);
            return View(viewModel);
        }
    }
}

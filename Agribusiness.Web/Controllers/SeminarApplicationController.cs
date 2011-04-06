using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using System.Web;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the SeminarApplication class
    /// </summary>
    public class SeminarApplicationController : ApplicationController
    {
        private readonly IRepository<Application> _applicationRepository;
        private readonly IFirmService _firmService;
        private readonly ISeminarService _seminarService;

        public SeminarApplicationController(IRepository<Application> applicationRepository, IFirmService firmService, ISeminarService seminarService)
        {
            _applicationRepository = applicationRepository;
            _firmService = firmService;
            _seminarService = seminarService;
        }

        [UserOnly]
        public ActionResult Index()
        {
            var applications = _applicationRepository.Queryable.OrderBy(a=>a.IsPending).ThenBy(a=>a.IsApproved);
            return View(applications);
        }

        [UserOnly]
        public ActionResult Decide(int id)
        {
            var application = _applicationRepository.GetNullableById(id);
            if (application == null)
            {
                Message = string.Format(Messages.NotFound, "application", id);
                return this.RedirectToAction(a => a.Index());
            }

            return View(application);
        }

        [HttpPost]
        [UserOnly]
        public ActionResult Decide(int id, bool isApproved)
        {
            var application = _applicationRepository.GetNullableById(id);
            if (application == null)
            {
                Message = string.Format(Messages.NotFound, "application", id);
                return this.RedirectToAction(a => a.Index());
            }

            // make the changes to the object
            application.IsPending = false;
            application.IsApproved = isApproved;
            application.DateDecision = DateTime.Now;

            application.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _seminarService.CreateSeminarPerson(application, ModelState);
            }

            // check if model state is still valid, might have changed on create
            if (ModelState.IsValid)
            {
                _applicationRepository.EnsurePersistent(application);
                Message = string.Format("Application for {0} has been {1}", application.FullName, isApproved ? "Approved" : "Denied");
                return this.RedirectToAction<SeminarApplicationController>(a => a.Index());
            }

            return View(application);
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
            ModelState.Clear();

            application.Seminar = _seminarService.GetCurrent();
            application.User = Repository.OfType<User>().Queryable.Where(a => a.LoweredUserName == CurrentUser.Identity.Name.ToLower()).FirstOrDefault();

            if (file != null)
            {
                // read the file
                var reader = new BinaryReader(file.InputStream);
                application.Photo = reader.ReadBytes(file.ContentLength);
                application.ContentType = file.ContentType;
            }

            application.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _applicationRepository.EnsurePersistent(application);
                Message = string.Format(Messages.Saved, "Application");
                return this.RedirectToAction<AuthorizedController>(a => a.Index());
            }

            var viewModel = ApplicationViewModel.Create(Repository, _firmService, CurrentUser.Identity.Name, application);
            return View(viewModel);
        }
    }
}

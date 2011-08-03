using System;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Services;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;
using MvcContrib;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// All public facing static pages.
    /// </summary>
    public class PublicController : ApplicationController
    {
        private readonly IRepository<InformationRequest> _informationRequestRepository;
        private readonly ISeminarService _seminarService;

        public PublicController(IRepository<InformationRequest> informationRequestRepository, ISeminarService seminarService)
        {
            _informationRequestRepository = informationRequestRepository;
            _seminarService = seminarService;
        }

        public ActionResult Background()
        {
            return this.RedirectToAction<HomeController>(a => a.ComingSoon());

            return View();
        }

        public ActionResult SteeringCommittee()
        {
            return this.RedirectToAction<HomeController>(a => a.ComingSoon());
            return View();
        }

        public ActionResult ProgramOverview()
        {
            return this.RedirectToAction<HomeController>(a => a.ComingSoon());
            return View();
        }

        public ActionResult CaseExamples()
        {
            return this.RedirectToAction<HomeController>(a => a.ComingSoon());
            return View();
        }

        public ActionResult Venue()
        {
            return this.RedirectToAction<HomeController>(a => a.ComingSoon());
            return View();
        }

        public ActionResult ContactUs()
        {
            return this.RedirectToAction<HomeController>(a => a.ComingSoon());
            return View();
        }

        public ActionResult MoreInformation()
        {
            return this.RedirectToAction<HomeController>(a => a.ComingSoon());
            return View(new InformationRequest());
        }

        [CaptchaValidator]
        [HttpPost]
        public ActionResult MoreInformation(InformationRequest informationRequest)
        {
            return this.RedirectToAction<HomeController>(a => a.ComingSoon());
            ModelState.Clear();

            informationRequest.Seminar = _seminarService.GetCurrent();
            informationRequest.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _informationRequestRepository.EnsurePersistent(informationRequest);
                Message = string.Format("Your request for information has been submitted.");
                return this.RedirectToAction<HomeController>(a => a.Index());
            }

            return View(informationRequest);
        }
    }

}

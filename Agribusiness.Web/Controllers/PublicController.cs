using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.App_GlobalResources;
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
        private readonly IRepositoryWithTypedId<SeminarRole, string> _seminarRoleRepository;
        private readonly ISeminarService _seminarService;

        public PublicController(IRepository<InformationRequest> informationRequestRepository, IRepositoryWithTypedId<SeminarRole, string> seminarRoleRepository , ISeminarService seminarService)
        {
            _informationRequestRepository = informationRequestRepository;
            _seminarRoleRepository = seminarRoleRepository;
            _seminarService = seminarService;
        }

        public ActionResult Background()
        {
            return View();
        }

        public ActionResult SteeringCommittee()
        {
            var seminar = _seminarService.GetCurrent();
            var role = _seminarRoleRepository.GetNullableById(StaticIndexes.Role_SteeringCommittee);

            if (seminar != null && role != null)
            {
                var committee = seminar.SeminarPeople.Where(a => a.SeminarRoles.Contains(role)).OrderBy(a => a.Person.LastName);

                return View(committee.ToList());
            }

            return View();
        }

        public ActionResult ProgramOverview()
        {
            return View();
        }

        public ActionResult CaseExamples()
        {
            return View();
        }

        public ActionResult Venue()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult MoreInformation()
        {
            return View(new InformationRequest());
        }

        [CaptchaValidator]
        [HttpPost]
        public ActionResult MoreInformation(InformationRequest informationRequest)
        {
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

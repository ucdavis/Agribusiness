using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
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
        private readonly IRepository<CaseStudy> _caseStudyRepository;
        private readonly ISeminarService _seminarService;

        public PublicController(IRepository<InformationRequest> informationRequestRepository, IRepositoryWithTypedId<SeminarRole, string> seminarRoleRepository, IRepository<CaseStudy> caseStudyRepository, ISeminarService seminarService)
        {
            _informationRequestRepository = informationRequestRepository;
            _seminarRoleRepository = seminarRoleRepository;
            _caseStudyRepository = caseStudyRepository;
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

                return View(committee.Select(a => new DisplayPerson() {Firm = a.Firm, Person = a.Person, Title = a.Title, Seminar = seminar}).ToList());
            }

            return View();
        }

        public ActionResult ProgramOverview()
        {
            return View();
        }

        /// <summary>
        /// Display a list of all previous case examples
        /// </summary>
        /// <returns></returns>
        public ActionResult CaseExamples()
        {
            var viewModel = CaseExampleViewModel.Create(_caseStudyRepository, _seminarService);

            return View(viewModel);
        }

        /// <summary>
        /// Information on the Seminar venue
        /// </summary>
        /// <returns></returns>
        public ActionResult Venue()
        {
            return View();
        }

        /// <summary>
        /// Contact information for organizers
        /// </summary>
        /// <returns></returns>
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

        public ActionResult GetPublicThumbnail(int id)
        {
            var person = Repository.OfType<Person>().GetById(id);

            if (person.MainProfilePicture != null) return File(person.ThumbnailPicture, person.ContentType);

            // check to make sure it is ok
            // for now only committee members can be downloaded from this action
            var committeeRole = Repository.OfType<SeminarRole>().Queryable.Where(a => a.Id == StaticIndexes.Role_SteeringCommittee).FirstOrDefault();
            var isCommitteeMember = Repository.OfType<SeminarPerson>().Queryable.Where(a => a.SeminarRoles.Contains(committeeRole) && a.Person == person).Any();

            // not authorized to release this peroson's image
            if (!isCommitteeMember) return File(new byte[0], string.Empty);

            // load the default image
            var fs = new FileStream(Server.MapPath("~/Images/profileplaceholder_thumb.jpg"), FileMode.Open, FileAccess.Read);
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();

            return File(img, "image/jpeg");
        }
    }

}

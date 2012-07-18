using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Helpers;
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
        private readonly INotificationService _notificationService;

        public PublicController(IRepository<InformationRequest> informationRequestRepository, IRepositoryWithTypedId<SeminarRole, string> seminarRoleRepository, IRepository<CaseStudy> caseStudyRepository, ISeminarService seminarService, INotificationService notificationService)
        {
            _informationRequestRepository = informationRequestRepository;
            _seminarRoleRepository = seminarRoleRepository;
            _caseStudyRepository = caseStudyRepository;
            _seminarService = seminarService;
            _notificationService = notificationService;
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
            var seminar = _seminarService.GetCurrent();

            return View(seminar.Sessions.Where(a => a.ShowPublic).ToList());
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

        /// <summary>
        /// Display a short bio and picture of the current Academic Director for the seminar
        /// </summary>
        /// <param name="id">Id of the person we want to display</param>
        /// <returns></returns>
        public ActionResult Profile(int id)
        {
            // load up the person
            var person = Repository.OfType<Person>().GetNullableById(id);

            if (person != null)
            {
                // get the last registration
                var seminarPerson = person.GetLatestRegistration();

                // is this person in the public roles of either faculty directory or steering committee?
                var roles = seminarPerson.SeminarRoles.Select(a => a.Id);
                if (roles.Contains(StaticIndexes.Role_FacultyDirector))
                {
                    ViewBag.Title = "Academic Director";

                    return View(seminarPerson);
                }

                if (roles.Contains(StaticIndexes.Role_SteeringCommittee))
                {
                    ViewBag.Title = "Steering Committee Member";

                    return View(seminarPerson);
                }
            }

            // unkown error, but really, trying to access profile that should not be public
            return this.RedirectToAction<ErrorController>(a => a.Index());
        }

        /// <summary>
        /// Page for people to request more information
        /// </summary>
        /// <returns></returns>
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

                // send the information request notification to admin
                _notificationService.SendInformationRequestNotification(informationRequest);

                // queue an email for the person requesting information
                _notificationService.SendInformationRequestConfirmatinon(informationRequest.Email);

                return this.RedirectToAction<HomeController>(a => a.Index());
            }

            return View(informationRequest);
        }

        public ActionResult GetPublicThumbnail(int? id)
        {
            if(id != null)
            {
                var person = Repository.OfType<Person>().GetById(id.Value);


                if (person.MainProfilePicture != null)
                    return File(person.ThumbnailPicture, person.ContentType ?? "image/png");

                // check to make sure it is ok
                // for now only committee members can be downloaded from this action
                var committeeRole = Repository.OfType<SeminarRole>().Queryable.Where(a => a.Id == StaticIndexes.Role_SteeringCommittee).FirstOrDefault();
                var isCommitteeMember = Repository.OfType<SeminarPerson>().Queryable.Where(a => a.SeminarRoles.Contains(committeeRole) && a.Person == person).Any();

                // not authorized to release this person's image
                if (!isCommitteeMember)
                    return File(new byte[0], string.Empty);
            }
            // load the default image
            var fs = new FileStream(Server.MapPath("~/Images/profileplaceholder_thumb.png"), FileMode.Open, FileAccess.Read);
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();

            return File(img, "image/png");
        }

        public ActionResult GetPublicPicture(int? id)
        {
            if(id != null)
            {
                var person = Repository.OfType<Person>().GetById(id.Value);


                // no result anyways
                if (person == null)
                {
                    return File(new byte[0], string.Empty);
                }

                // for now only committee members can be downloaded from this action
                var committeeRole = Repository.OfType<SeminarRole>().Queryable.Where(a => a.Id == StaticIndexes.Role_SteeringCommittee).FirstOrDefault();
                var isCommitteeMember = Repository.OfType<SeminarPerson>().Queryable.Where(a => a.SeminarRoles.Contains(committeeRole) && a.Person == person).Any();

                // not authorized to release this person's image
                if (!isCommitteeMember)
                    return File(new byte[0], string.Empty);

                // has picture return that
                if (person.MainProfilePicture != null)
                    return File(person.MainProfilePicture, person.ContentType ?? "image/png");
            }
            // load the default image
            var fs = new FileStream(Server.MapPath("~/Images/profilepicplaceholder.png"), FileMode.Open, FileAccess.Read);
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();

            return File(img, "image/png");
        }
    }

}

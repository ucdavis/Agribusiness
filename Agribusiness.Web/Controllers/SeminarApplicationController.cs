﻿using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Repositories;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.ActionResults;
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
        private readonly INotificationService _notificationService;
        private readonly IEventService _eventService;
        private readonly IPictureService _pictureService;
        private readonly IPersonService _personService;
        private readonly IRepositoryFactory _repositoryFactory;

        public SeminarApplicationController(IRepository<Application> applicationRepository, IFirmService firmService, INotificationService notificationService, IEventService eventService, IPictureService pictureService, IPersonService personService, IRepositoryFactory repositoryFactory)
        {
            _applicationRepository = applicationRepository;
            _firmService = firmService;
            _notificationService = notificationService;
            _eventService = eventService;
            _pictureService = pictureService;
            _personService = personService;
            _repositoryFactory = repositoryFactory;
        }

        [UserOnly]
        public ActionResult Index()
        {
            var seminar = SiteService.GetLatestSeminar(Site);
            var applications = _applicationRepository.Queryable.Where(a => a.Seminar == seminar).OrderBy(a=>a.IsPending).ThenBy(a=>a.IsApproved);
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
        public ActionResult Decide(int id, bool isApproved, string reason)
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
            application.DecisionReason = reason;

            application.TransferValidationMessagesTo(ModelState);

            Person person = null;

            if (ModelState.IsValid)
            {
                person = _personService.CreateSeminarPerson(application, ModelState);
            }

            if (person == null)
            {
                ModelState.AddModelError("Person", "Unsuccessful in adding person to the current seminar.");
            }

            // check if model state is still valid, might have changed on create
            if (ModelState.IsValid)
            {
                // save the application
                _applicationRepository.EnsurePersistent(application);
                Message = string.Format("Application for {0} has been {1}", application.FullName, isApproved ? "Approved" : "Denied");

                if (isApproved)
                {
                    _eventService.Accepted(person, Site);
                }
                else
                {
                   _eventService.Denied(person, Site);
                }

                return this.RedirectToAction<SeminarApplicationController>(a => a.Index());
            }

            return View(application);
        }

        [HttpPost]
        [UserOnly]
        public JsonNetResult SaveComments(int id, string comments)
        {
            var application = _applicationRepository.GetNullableById(id);
            if (application == null) return new JsonNetResult(string.Format(Messages.NotFound, "application", id));

            try
            {
                application.DecisionReason = comments;
                _applicationRepository.EnsurePersistent(application);

                return new JsonNetResult(string.Format(Messages.Saved, "Decision Reason"));
            }
            catch (Exception ex)
            {
                return new JsonNetResult("There was an error saving the comments, please reload page and try again.");
            }
        }

        #region Membership User Functions
        [MembershipUserOnly]
        public ActionResult Apply()
        {
            var viewModel = ApplicationViewModel.Create(Repository, _firmService, CurrentUser.Identity.Name, Site);
            return View(viewModel);
        }

        [HttpPost]
        [MembershipUserOnly]
        public ActionResult Apply(Application application, HttpPostedFileBase file, bool seminarTerms)
        {
            ModelState.Clear();

            application.Seminar = SiteService.GetLatestSeminar(Site, true);
            application.User = Repository.OfType<User>().Queryable.FirstOrDefault(a => a.LoweredUserName == CurrentUser.Identity.Name.ToLower());

            // requires assistant
            if (application.CommunicationOption == null)
            {
                ModelState.AddModelError("Communication Option", "No communication option was selected.");
            }
            else
            {
                if (application.CommunicationOption.RequiresAssistant)
                {
                    if (string.IsNullOrWhiteSpace(application.FirstName) && string.IsNullOrWhiteSpace(application.LastName))
                    {
                        ModelState.AddModelError("Assistant Name", "Because of your communication preference an Assistant Name is required.");
                    }

                    if (string.IsNullOrWhiteSpace(application.AssistantPhone))
                    {
                        ModelState.AddModelError("Assistant Name", "Because of your communication preference an Assistant Phone is required.");
                    }

                    if (string.IsNullOrWhiteSpace(application.AssistantEmail))
                    {
                        ModelState.AddModelError("Assistant Name", "Because of your communication preference an Assistant Email is required.");
                    }
                }    
            }
            

            if (file != null)
            {
                // read the file
                var reader = new BinaryReader(file.InputStream);
                application.Photo = reader.ReadBytes(file.ContentLength);
                application.ContentType = file.ContentType;
            }

            if (application.Firm != null && application.Firm.Id == 0)
            {
                application.Firm = null;
            }

            application.TransferValidationMessagesTo(ModelState);

            //var test = application.Firm;
            //ModelState.AddModelError("Firm", "Please define a firm");

            if (application.FirmType != null && application.FirmType.Name == "Other" && string.IsNullOrWhiteSpace(application.OtherFirmType))
            {
                ModelState.AddModelError("Firm Type", "Please define Firm Type.");
            }

            if (!seminarTerms) ModelState.AddModelError("Seminar Terms", "Agreeing to the seminar terms are required.");

            if (ModelState.IsValid)
            {
                _applicationRepository.EnsurePersistent(application);

                _eventService.Apply(application.User.Person, application, Site);

                if (application.Seminar.RequireApproval)
                {
                    if (application.Seminar.AcceptanceDate.HasValue)
                    {
                        Message = string.Format("Thank you for successfully submitting your application.<br/>  Applicants will be notified of acceptance by {0}", application.Seminar.AcceptanceDate.Value.ToString("d"));
                    }
                    else
                    {
                        Message = "Thank you for successfully submitting your application.<br/>  Applicants will be notified in the near future.";
                    }    
                }
                else
                {
                    application.IsPending = false;
                    application.IsApproved = true;
                    application.DateDecision = DateTime.Now;
                    application.DecisionReason = "Approval not required.";

                    var person = _personService.CreateSeminarPerson(application, ModelState);
                    _applicationRepository.EnsurePersistent(application);

                    _eventService.Accepted(person, Site);
                }

                if (application.Photo != null)
                {
                    return this.RedirectToAction("UpdateProfilePicture", "Person", new {id = application.User.Person.Id});
                }

                return this.RedirectToAction<AuthorizedController>(a => a.Index());
            }

            var viewModel = ApplicationViewModel.Create(Repository, _firmService, CurrentUser.Identity.Name, Site, application, seminarTerms);
            return View(viewModel);
        }
        #endregion

        /// <summary>
        /// Gets photo from an application
        /// </summary>
        /// <param name="id">Application Id</param>
        /// <returns></returns>
        public FileResult GetApplicationPhoto(int id)
        {
            var application = _applicationRepository.GetNullableById(id);

            byte[] photo = null;
            var contentType = "image/jpeg";

            // application exists
            if (application != null)
            {
                // application does not have a photo
                if (application.Photo == null)
                {
                    var person = application.User.Person;

                    // attempt to assign person's existing main profile picture
                    if (person != null)
                    {
                        photo = person.MainProfilePicture;
                        contentType = string.IsNullOrWhiteSpace(person.ContentType) ? contentType : person.ContentType;
                    }
                }
                else
                {
                    photo = _pictureService.MakeMainProfile(application.Photo);
                    contentType = string.IsNullOrWhiteSpace(application.ContentType) ? contentType : application.ContentType;
                }
            }

            // no photo anywhere, give the placeholder
            if (photo == null)
            {
                // load the default image
                var fs = new FileStream(Server.MapPath("~/Images/profilepicplaceholder.png"), FileMode.Open, FileAccess.Read);
                var img = new byte[fs.Length];
                fs.Read(img, 0, img.Length);
                fs.Close();

                return File(img, "image/jpeg");    
            }

            return File(photo, contentType);
        }

        public FileResult DownloadOriginalPhoto(int id)
        {
            byte[] photo = null;
            var contentType = "image/jpeg";

            var application = _applicationRepository.GetNullableById(id);
            if (application != null)
            {
                var person = application.User.Person;

                // attempt to assign person's existing main profile picture
                if (person != null)
                {
                    photo = person.OriginalPicture;
                    contentType = string.IsNullOrWhiteSpace(person.ContentType) ? contentType : person.ContentType;
                    if (photo == null)
                    {
                        photo = person.MainProfilePicture;
                    }
                }
                if (photo == null)
                {
                    photo = application.Photo;
                    contentType = string.IsNullOrWhiteSpace(application.ContentType) ? contentType : application.ContentType;
                }
            }

            return File(photo, contentType, string.Format("AgribusinessPhoto_{0}.jpg", application.FullName)); 
        }
    }
}

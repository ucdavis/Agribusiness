using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Resources;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.ActionResults;
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
        private readonly INotificationService _notificationService;

        public SeminarApplicationController(IRepository<Application> applicationRepository, IFirmService firmService, ISeminarService seminarService, INotificationService notificationService)
        {
            _applicationRepository = applicationRepository;
            _firmService = firmService;
            _seminarService = seminarService;
            _notificationService = notificationService;
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
                person = _seminarService.CreateSeminarPerson(application, ModelState);
            }

            // check if model state is still valid, might have changed on create
            if (ModelState.IsValid)
            {
                // save the application
                _applicationRepository.EnsurePersistent(application);
                Message = string.Format("Application for {0} has been {1}", application.FullName, isApproved ? "Approved" : "Denied");

                if (isApproved)
                {
                    _notificationService.AddToMailingList(application.Seminar, person, MailingLists.Registered);

                    // add user to the reminder emails
                    _notificationService.AddToMailingList(application.Seminar, person, MailingLists.PaymentReminder);
                    _notificationService.AddToMailingList(application.Seminar, person, MailingLists.HotelReminder);

                    if (string.IsNullOrWhiteSpace(person.Biography)) _notificationService.AddToMailingList(application.Seminar, person, MailingLists.BioReminder);
                    if (person.OriginalPicture == null) _notificationService.AddToMailingList(application.Seminar, person, MailingLists.PhotoReminder);

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

            // requires assistant
            if (application.CommunicationOption.RequiresAssistant)
            {
                if (string.IsNullOrWhiteSpace(application.AssistantName))
                {
                    ModelState.AddModelError("Assistant Name", "Becuase of your communication preference an Assistant Name is required.");
                }

                if (string.IsNullOrWhiteSpace(application.AssistantPhone))
                {
                    ModelState.AddModelError("Assistant Name", "Becuase of your communication preference an Assistant Phone is required.");
                }

                if (string.IsNullOrWhiteSpace(application.AssistantEmail))
                {
                    ModelState.AddModelError("Assistant Name", "Becuase of your communication preference an Assistant Email is required.");
                }
            }

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
        #endregion

        /// <summary>
        /// Gets photo from an application
        /// </summary>
        /// <param name="id">Application Id</param>
        /// <returns></returns>
        public FileResult GetApplicationPhoto(int id)
        {
            var application = _applicationRepository.GetNullableById(id);

            if (application == null || application.Photo == null)
            {
                // load the default image
                var fs = new FileStream(Server.MapPath("~/Images/profilepicplaceholder.jpg"), FileMode.Open, FileAccess.Read);
                var img = new byte[fs.Length];
                fs.Read(img, 0, img.Length);
                fs.Close();

                return File(img, "image/jpeg");    
            }

            return File(application.Photo, application.ContentType);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Resources;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using AutoMapper;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using UCDArch.Web.Attributes;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Notification class
    /// </summary>
    [UserOnly]
    public class NotificationController : ApplicationController
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Seminar> _seminarRepository;
        private readonly IRepository<NotificationTracking> _notificationTrackingRepository;
        private readonly IRepositoryWithTypedId<NotificationMethod, string> _notificationMethodRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly ISeminarService _seminarService;
        private readonly INotificationService _notificationService;

        public NotificationController(IRepository<Person> personRepository, IRepository<Seminar> seminarRepository, IRepository<NotificationTracking> notificationTrackingRepository
                                    , IRepositoryWithTypedId<NotificationMethod, string> notificationMethodRepository, IRepository<Attachment> attachmentRepository
                                    , ISeminarService seminarService, INotificationService notificationService)
        {
            _personRepository = personRepository;
            _seminarRepository = seminarRepository;
            _notificationTrackingRepository = notificationTrackingRepository;
            _notificationMethodRepository = notificationMethodRepository;
            _attachmentRepository = attachmentRepository;
            _seminarService = seminarService;
            _notificationService = notificationService;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action for adding a tracking object, for notifications sent outside of the program
        /// </summary>
        /// <param name="personId">If sending to a specific person</param>
        /// <returns></returns>
        public ActionResult Create(int? personId)
        {
            Person person = null;
            if (personId.HasValue)
            {
                person = _personRepository.GetNullableById(personId.Value);
            }

            var viewModel = NotificationTrackingViewModel.Create(Repository, _seminarService, person: person);
            
            viewModel.NotificationTracking.NotifiedBy = CurrentUser.Identity.Name;
            viewModel.NotificationTracking.Seminar = _seminarService.GetCurrent();
            
            return View(viewModel);
        }

        /// <summary>
        /// Create a notification tracking object
        /// </summary>
        /// <param name="seminarId"></param>
        /// <param name="notificationTracking"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(List<int> people, NotificationTracking notificationTracking)
        {
            if (people == null || people.Count <= 0)
            {
                ModelState.AddModelError("People", "No person has been selected to receive the notification.");
            }

            var tracking = new List<NotificationTracking>();

            if (ModelState.IsValid)
            {
                tracking = ProcessTracking(ModelState, people, notificationTracking, new int[0]);

                foreach (var a in tracking)
                {
                    _notificationTrackingRepository.EnsurePersistent(a);
                }
                
                Message = string.Format(Messages.Saved, "Notification tracking");

                if (tracking.Count == 1)
                {
                    var person = tracking[0].Person;

                    var url = Url.Action("AdminEdit", "Person", new { id = person.User.Id, seminarId = _seminarService.GetCurrent().Id });
                    return Redirect(string.Format("{0}#notifications", url));
                }
                
                // redirect back to the seminar controller details
                return this.RedirectToAction<SeminarController>(a => a.Details(null));
            }

            var viewModel = NotificationTrackingViewModel.Create(Repository, _seminarService, notificationTracking);
            viewModel.People = tracking.Select(a=>a.Person).ToList();

            return View(viewModel);
        }

        /// <summary>
        /// Sending a notification through this program
        /// </summary>
        /// <returns></returns>
        public ActionResult Send(int? personId)
        {
            Person person = null;
            if (personId.HasValue)
            {
                person = _personRepository.GetNullableById(personId.Value);
            }

            var ntViewModel = NotificationTrackingViewModel.Create(Repository, _seminarService, person: person);

            ntViewModel.NotificationTracking.NotifiedBy = CurrentUser.Identity.Name;
            ntViewModel.NotificationTracking.Seminar = _seminarService.GetCurrent();
            ntViewModel.NotificationTracking.NotificationMethod = _notificationMethodRepository.GetById(StaticIndexes.NotificationMethod_Email);

            var viewModel = SendNotificationViewModel.Create(Repository, ntViewModel);
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Send(List<int> people, NotificationTracking notificationTracking, EmailQueue emailQueue, int? mailingListId, int[] attachmentIds)
        {
            if ((people == null || people.Count <= 0) && !mailingListId.HasValue)
            {
                ModelState.AddModelError("People", "No person has been selected to receive the notification.");
            }

            var tracking = new List<NotificationTracking>();

            ModelState.Clear();
            notificationTracking.TransferValidationMessagesTo(ModelState);

            var mailingList = mailingListId.HasValue ? Repository.OfType<MailingList>().GetNullableById(mailingListId.Value) : null;

            // save the objects if we are good
            if (ModelState.IsValid)
            {
                tracking = ProcessTracking(ModelState, people, notificationTracking, attachmentIds, emailQueue, mailingList);

                foreach (var a in tracking)
                {
                    _notificationTrackingRepository.EnsurePersistent(a);
                }

                Message = string.Format(Messages.Saved, "Notification tracking");

                if (tracking.Count == 1)
                {
                    var person = tracking[0].Person;

                    var url = Url.Action("AdminEdit", "Person", new { id = person.User.Id, seminarId = _seminarService.GetCurrent().Id });
                    return Redirect(string.Format("{0}#notifications", url));
                }

                // redirect back to the seminar controller details
                return this.RedirectToAction<SeminarController>(a => a.Details(null));
            }

            // not good, hand the page back
            var ntViewModel = NotificationTrackingViewModel.Create(Repository, _seminarService, notificationTracking, mailingList:mailingList );
            ntViewModel.People = tracking.Select(a => a.Person).ToList();

            var viewModel = SendNotificationViewModel.Create(Repository, ntViewModel, emailQueue);
            return View(viewModel);
        }
     
        [HttpPost]
        [BypassAntiForgeryToken]
        public JsonResult SaveAttachment()
        {
            try
            {

                var request = ControllerContext.HttpContext.Request;
                var fileName = request["qqfile"];
                var contentType = request.Headers["X-File-Type"];

                byte[] contents;

                using (var reader = new BinaryReader(request.InputStream))
                {
                    contents = reader.ReadBytes((int) request.InputStream.Length);
                }

                // save the attachment
                var attachment = new Attachment() {Contents = contents, ContentType = contentType, FileName = fileName};
                _attachmentRepository.EnsurePersistent(attachment);

                return Json(new {id = attachment.Id, fileName = fileName, success=true});
            }
            catch (Exception)
            {
                return Json(false);
            }

            return Json(false);
        }

        /// <summary>
        /// Create all the notification tracking objects and email queue if provided
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="people"></param>
        /// <param name="notificationTracking"></param>
        /// <param name="emailQueue">(Optional)</param>
        /// <returns></returns>
        protected List<NotificationTracking> ProcessTracking(ModelStateDictionary modelState, List<int> people, NotificationTracking notificationTracking, int[] attachmentIds, EmailQueue emailQueue = null, MailingList mailingList = null)
        {
            Check.Require(people != null || mailingList != null, "people is required.");
            Check.Require(notificationTracking != null, "notificationTracking is required.");

            mailingList = mailingList ?? new MailingList();
            people = people ?? new List<int>();

            // build the list of people to send to
            var peeps = _personRepository.Queryable.Where(a => people.Contains(a.Id)).ToList();
            peeps.AddRange(mailingList.People);

            var tracking = new List<NotificationTracking>();

            foreach (var person in peeps.Distinct())
            {
                var nt = new NotificationTracking();
                // copy the fields
                Mapper.Map(notificationTracking, nt);
                nt.Seminar = notificationTracking.Seminar;
                // assign the person
                nt.Person = person;

                if (emailQueue != null)
                {
                    var eq = new EmailQueue();

                    Mapper.Map(emailQueue, eq);

                    Invitation invitation = null;
                    if (mailingList.Name == MailingLists.Invitation)
                    {
                        // get the invitation object
                        invitation = Repository.OfType<Invitation>().Queryable.Where(a => a.Person == person && a.Seminar == notificationTracking.Seminar).FirstOrDefault();
                    }

                    eq.Body = _notificationService.GenerateNotification(eq.Body, person, notificationTracking.Seminar.Id, invitation);
                    
                    // add attachments
                    var attachments = _attachmentRepository.Queryable.Where(a => attachmentIds.Contains(a.Id)).ToList();
                    foreach(var a in attachments) eq.Attachments.Add(a);
                    
                    eq.Person = person;
                    nt.EmailQueue = eq;
                }

                // add it to the list
                tracking.Add(nt);
            }

            // add errors for those not in the list
            foreach (var id in people.Where(x => !peeps.Select(a => a.Id).Contains(x)))
            {
                ModelState.AddModelError("Person", string.Format("Person with id {0} could not be found.", id));
            }

            return tracking;
        }
    }
}
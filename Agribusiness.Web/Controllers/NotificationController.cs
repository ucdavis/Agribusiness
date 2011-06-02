using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using AutoMapper;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Notification class
    /// </summary>
    public class NotificationController : ApplicationController
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Seminar> _seminarRepository;
        private readonly IRepository<NotificationTracking> _notificationTrackingRepository;
        private readonly IRepositoryWithTypedId<NotificationMethod, string> _notificationMethodRepository;
        private readonly ISeminarService _seminarService;

        public NotificationController(IRepository<Person> personRepository, IRepository<Seminar> seminarRepository, IRepository<NotificationTracking> notificationTrackingRepository
                                    , IRepositoryWithTypedId<NotificationMethod, string> notificationMethodRepository
                                    , ISeminarService seminarService)
        {
            _personRepository = personRepository;
            _seminarRepository = seminarRepository;
            _notificationTrackingRepository = notificationTrackingRepository;
            _notificationMethodRepository = notificationMethodRepository;
            _seminarService = seminarService;
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

            var peeps = new List<Person>();

            if (ModelState.IsValid)
            {
                foreach (var a in people)
                {
                    var person = _personRepository.GetNullableById(a);

                    if (person == null)
                    {
                        ModelState.AddModelError("Person", string.Format("Person with id {0} could not be found.", a));
                    }
                    else
                    {
                        peeps.Add(person);

                        var nt = new NotificationTracking();
                        // copy the fields
                        Mapper.Map(notificationTracking, nt);
                        nt.Seminar = notificationTracking.Seminar;
                        // assign the person
                        nt.Person = person;
                        // add it to the list
                        _notificationTrackingRepository.EnsurePersistent(nt);
                    }
                }

                Message = string.Format(Messages.Saved, "Notification tracking");

                if (peeps.Count == 1)
                {
                    var person = peeps[0];

                    var url = Url.Action("AdminEdit", "Person", new { id = person.User.Id, seminarId = _seminarService.GetCurrent().Id });
                    return Redirect(string.Format("{0}#notifications", url));
                }
                
                // redirect back to the seminar controller details
                return this.RedirectToAction<SeminarController>(a => a.Details(null));
            }

            var viewModel = NotificationTrackingViewModel.Create(Repository, _seminarService, notificationTracking);
            viewModel.People = peeps;

            return View(viewModel);
        }

        /*
        /// <summary>
        /// Sending a notification through this program
        /// </summary>
        /// <returns></returns>
        public ActionResult Send(int personId, int seminarId)
        {
            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", personId);
                return this.RedirectToAction<PersonController>(a => a.Index());
            }

            var ntViewModel = NotificationTrackingViewModel.Create(Repository, person, seminarId, person.User.Id);
            ntViewModel.NotificationTracking.NotifiedBy = CurrentUser.Identity.Name;
            ntViewModel.NotificationTracking.Seminar = _seminarService.GetCurrent();

            ntViewModel.NotificationTracking.NotificationMethod = _notificationMethodRepository.GetById(StaticIndexes.NotificationMethod_Email);

            var viewModel = SendNotificationViewModel.Create(Repository, ntViewModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Send(int personId, int seminarId, NotificationTracking notificationTracking, EmailQueue emailQueue)
        {
            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", personId);
                return this.RedirectToAction<PersonController>(a => a.Index());
            }

            notificationTracking.Person = person;
            notificationTracking.EmailQueue = emailQueue;

            ModelState.Clear();
            notificationTracking.TransferValidationMessagesTo(ModelState);
            emailQueue.TransferValidationMessagesTo(ModelState);

            // save the objects if we are good););
            if (ModelState.IsValid)
            {
                
            }

            // not good, hand the page back
            var ntViewModel = NotificationTrackingViewModel.Create(Repository, person, seminarId, person.User.Id, notificationTracking);
            ntViewModel.NotificationTracking.NotifiedBy = CurrentUser.Identity.Name;
            ntViewModel.NotificationTracking.Seminar = _seminarService.GetCurrent();

            var viewModel = SendNotificationViewModel.Create(Repository, ntViewModel, emailQueue);
            return View(viewModel);
        }
         */
    }
}
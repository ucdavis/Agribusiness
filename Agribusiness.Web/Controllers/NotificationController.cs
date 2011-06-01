using System;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
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
        private readonly ISeminarService _seminarService;

        public NotificationController(IRepository<Person> personRepository, IRepository<Seminar> seminarRepository, IRepository<NotificationTracking> notificationTrackingRepository, ISeminarService seminarService)
        {
            _personRepository = personRepository;
            _seminarRepository = seminarRepository;
            _notificationTrackingRepository = notificationTrackingRepository;
            _seminarService = seminarService;
        }

        public ActionResult Create(int personId, int seminarId)
        {
            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", personId);
                return this.RedirectToAction<PersonController>(a => a.Index());
            }

            var viewModel = NotificationTrackingViewModel.Create(Repository, person.Id, seminarId, person.User.Id);
            viewModel.NotificationTracking.NotifiedBy = CurrentUser.Identity.Name;
            viewModel.NotificationTracking.Seminar = _seminarService.GetCurrent();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(int personId, int seminarId, NotificationTracking notificationTracking)
        {
            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", personId);
                return this.RedirectToAction<PersonController>(a => a.Index());
            }

            notificationTracking.Person = person;


            if (ModelState.IsValid)
            {
                _notificationTrackingRepository.EnsurePersistent(notificationTracking);

                Message = string.Format(Messages.Saved, "Notification tracking");

                var url = Url.Action("AdminEdit", "Person", new { id = person.User.Id, seminarId = seminarId });
                return Redirect(string.Format("{0}#notifications", url));
            }

            var viewModel = NotificationTrackingViewModel.Create(Repository, person.Id, seminarId, person.User.Id, notificationTracking);
            return View(viewModel);
        }
    }
}

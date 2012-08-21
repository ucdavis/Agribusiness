using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Resources;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using AutoMapper;
using Resources;
using UCDArch.Core.PersistanceSupport;
using MvcContrib;
using UCDArch.Web.Helpers;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Seminar class
    /// </summary>
    public class SeminarController : ApplicationController
    {
	    private readonly IRepository<Seminar> _seminarRepository;
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;
        private readonly IRepository<MailingList> _mailingListRepository;
        private readonly IPersonService _personService;

        public SeminarController(IRepository<Seminar> seminarRepository, IRepository<SeminarPerson> seminarPersonRepository, IRepository<MailingList> mailingListRepository, IPersonService personService)
        {
            _seminarRepository = seminarRepository;
            _seminarPersonRepository = seminarPersonRepository;
            _mailingListRepository = mailingListRepository;
            _personService = personService;
        }

        #region Administrative Functions
        //
        // GET: /Seminar/
        [UserOnly]
        public ActionResult Index()
        {
            var seminarList = _seminarRepository.Queryable.OrderByDescending(a => a.Year);

            return View(seminarList);
        }

        [UserOnly]
        public ActionResult Create()
        {
            var viewModel = SeminarViewModel.Create(Repository, SiteService.LoadSite(Site));

            return View(viewModel);
        }

        [UserOnly]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Seminar seminar)
        {
            seminar.Site = SiteService.LoadSite(Site);
            ModelState.Clear();
            seminar.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _seminarRepository.EnsurePersistent(seminar);

                var mlInvitation = new MailingList(MailingLists.Invitation, seminar) {Description = MailingLists.InvitationDescription };
                var mlRegistered = new MailingList(MailingLists.Registered, seminar) {Description = MailingLists.RegisteredDescription };
                var mlAttending = new MailingList(MailingLists.Attending, seminar) {Description = MailingLists.AttendingDescription };
                var mlPaymentReminder = new MailingList(MailingLists.PaymentReminder, seminar) {Description = MailingLists.PaymentReminderDescription };
                var mlHotelReminder = new MailingList(MailingLists.HotelReminder, seminar) {Description = MailingLists.HotelReminderDescription };
                var mlPhotoReminder = new MailingList(MailingLists.PhotoReminder, seminar) {Description = MailingLists.PhotoReminderDescription };
                var mlBioReminder = new MailingList(MailingLists.BioReminder, seminar) {Description = MailingLists.BioReminderDescription };
                var mlApplied = new MailingList(MailingLists.Applied, seminar) {Description = MailingLists.AppliedDescription };
                var mlDenied = new MailingList(MailingLists.Denied, seminar) {Description = MailingLists.DeniedDescription };

                _mailingListRepository.EnsurePersistent(mlInvitation);
                _mailingListRepository.EnsurePersistent(mlRegistered);
                _mailingListRepository.EnsurePersistent(mlAttending);
                _mailingListRepository.EnsurePersistent(mlPaymentReminder);
                _mailingListRepository.EnsurePersistent(mlHotelReminder);
                _mailingListRepository.EnsurePersistent(mlPhotoReminder);
                _mailingListRepository.EnsurePersistent(mlBioReminder);
                _mailingListRepository.EnsurePersistent(mlApplied);
                _mailingListRepository.EnsurePersistent(mlDenied);

                Message = string.Format(Messages.Saved, "Seminar");
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = SeminarViewModel.Create(Repository, seminar.Site, seminar);
            return View(viewModel);
        }

        [UserOnly]
        public ActionResult Edit(int id)
        {
            var seminar = LoadSeminar(id);

            if (seminar == null)
            {
                ErrorMessages = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = SeminarViewModel.Create(Repository, seminar.Site, seminar);
            return View(viewModel);
        }

        [UserOnly]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Seminar seminar)
        {
            var origSeminar = LoadSeminar(id);

            if (origSeminar == null)
            {
                ErrorMessages = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction(a => a.Index());
            }

            Mapper.Map(seminar, origSeminar);

            if (ModelState.IsValid)
            {
                _seminarRepository.EnsurePersistent(origSeminar);
                Message = string.Format(Messages.Saved, "Seminar");
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = SeminarViewModel.Create(Repository, seminar.Site, seminar);
            return View(viewModel);
        }

        [UserOnly]
        public ActionResult Details(int? id)
        {
            var seminar = LoadSeminar(id);

            if (seminar == null)
            {
                ErrorMessages = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = SeminarViewModel.Create(Repository, seminar.Site, seminar);
            viewModel.IsCurrent = SiteService.GetLatestSeminar(Site).Id == seminar.Id;
            viewModel.DisplayPeople = _personService.GetDisplayPeopleForSeminar(seminar.Id);

            return View(viewModel);
        }

        /// <summary>
        /// Loads a seminar object
        /// </summary>
        /// <param name="id">when null, loads the currnet seminar (the latest one)</param>
        /// <returns></returns>
        private Seminar LoadSeminar(int? id)
        {
            Seminar seminar = null;

            if (id.HasValue) seminar = _seminarRepository.GetNullableById(id.Value);

            // if we found one, return it
            if (seminar != null) return seminar;

            // otherwise just load the latest one
            seminar = SiteService.GetLatestSeminar(Site);

            return seminar;
        }

        ///// <summary>
        ///// Assign people to sessions to attend.
        ///// </summary>
        ///// <param name="id">Seminar Id</param>
        ///// <returns></returns>
        //[UserOnly]
        //public ActionResult AssignToSessions(int id)
        //{
        //    var seminar = LoadSeminar(id);

        //    // redirect to the list if no seminar
        //    if (seminar == null)
        //    {
        //        ErrorMessages = string.Format(Messages.NotFound, "Seminar", id);
        //        return this.RedirectToAction(a => a.Index());
        //    }

        //    var viewModel = AssignToSessionViewModel.Create(seminar);
        //    return View(viewModel);
        //}

        ///// <summary>
        ///// Assigns a person into a session
        ///// </summary>
        ///// <param name="id">Session Id</param>
        ///// <param name="seminarPersonId"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[UserOnly]
        //public JsonResult Assign(int id, int seminarPersonId)
        //{
        //    var session = _sessionRepository.GetNullableById(id);
        //    var seminarPerson = _seminarPersonRepository.GetNullableById(seminarPersonId);

        //    // make sure the sesion isn't already assigned
        //    if (!seminarPerson.Sessions.Any(a => a == session))
        //    {
        //        seminarPerson.Sessions.Add(session);
        //        _seminarPersonRepository.EnsurePersistent(seminarPerson);

        //        return Json(true);
        //    }

        //    return Json(false);
        //}

        ///// <summary>
        ///// Removes a session from a person
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="seminarPersonId"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[UserOnly]
        //public JsonResult UnAssign(int id, int seminarPersonId)
        //{
        //    //var session = _sessionRepository.GetNullableById(id);
        //    var seminarPerson = _seminarPersonRepository.GetNullableById(seminarPersonId);

        //    var session = seminarPerson.Sessions.Where(a => a.Id == id).FirstOrDefault();
        //    seminarPerson.Sessions.Remove(session);

        //    _seminarPersonRepository.EnsurePersistent(seminarPerson);
        //    return Json(true);
        //}

        /// <summary>
        /// Gets the details for the popup balloons on assign to sessions
        /// </summary>
        /// <param name="id">Seminar Person</param>
        /// <returns></returns>
        public ActionResult GetDetails(int id)
        {
            var person = _seminarPersonRepository.GetNullableById(id);
            var firm = person.Firm;

            ViewBag.Person = person;
            ViewBag.Firm = firm;

            return View();
        }

        #endregion

        #region Membership User Functions
        /// <summary>
        /// Loads up the seminar information for a person's seminar
        /// </summary>
        /// <param name="id">Seminar Person Id</param>
        /// <returns></returns>
        [MembershipUserOnly]
        public ActionResult MySeminar(int id)
        {
            var seminarPerson = _seminarPersonRepository.GetNullableById(id);

            // invalid seminar person id
            if (seminarPerson == null)
            {
                Message = string.Format(Messages.NotFound, "seminar person", id);
                return this.RedirectToAction<AuthorizedController>(a => a.Index());
            }

            if (!_personService.HasAccess(seminarPerson.Person, seminarPerson.Seminar, false))
            {
                return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
            }

            // someone is trying to access seminar person of another user, tsk tsk
            if (seminarPerson.Person.User.LoweredUserName != CurrentUser.Identity.Name.ToLower())
            {
                return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
            }

            var viewModel = MySeminarViewModel.Create(Repository, seminarPerson);

            return View(viewModel);
        }
        #endregion

        public FileResult DownloadSchedule()
        {
            var seminar = SiteService.GetLatestSeminar(Site);
            if (seminar.ScheduleFile != null)
            {
                return File(seminar.ScheduleFile, seminar.ScheduleFileContentType);
            }

            return File(new byte[0], string.Empty);
        }

        public FileResult DownloadBrochure()
        {
            var seminar = SiteService.GetLatestSeminar(Site);
            if (seminar.BrochureFile != null)
            {
                return File(seminar.BrochureFile, seminar.BrochureFileContentType);
            }

            return File(new byte[0], string.Empty);
        }
    }
}

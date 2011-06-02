using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
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
    /// Controller for the Seminar class
    /// </summary>
    public class SeminarController : ApplicationController
    {
	    private readonly IRepository<Seminar> _seminarRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;
        private readonly IFirmService _firmService;
        private readonly IPersonService _personService;
        private readonly ISeminarService _seminarService;

        public SeminarController(IRepository<Seminar> seminarRepository, IRepository<Session> sessionRepository, IRepository<Person> personRepository, IRepository<SeminarPerson> seminarPersonRepository, IFirmService firmService, IPersonService personService, ISeminarService seminarService)
        {
            _seminarRepository = seminarRepository;
            _sessionRepository = sessionRepository;
            _personRepository = personRepository;
            _seminarPersonRepository = seminarPersonRepository;
            _firmService = firmService;
            _personService = personService;
            _seminarService = seminarService;
        }

        #region Administrative Functions
        //
        // GET: /Seminar/
        [UserOnly]
        public ActionResult Index()
        {
            var seminarList = _seminarRepository.Queryable;

            return View(seminarList);
        }

        [UserOnly]
        public ActionResult Create()
        {
            var viewModel = SeminarViewModel.Create(Repository);

            return View(viewModel);
        }

        [UserOnly]
        [HttpPost]
        public ActionResult Create(Seminar seminar)
        {
            if (ModelState.IsValid)
            {
                _seminarRepository.EnsurePersistent(seminar);
                Message = string.Format(Messages.Saved, "Seminar");
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = SeminarViewModel.Create(Repository, seminar);
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

            var viewModel = SeminarViewModel.Create(Repository, seminar);
            return View(viewModel);
        }

        [UserOnly]
        [HttpPost]
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

            var viewModel = SeminarViewModel.Create(Repository, seminar);
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

            var viewModel = SeminarViewModel.Create(Repository, seminar);
           //viewModel.PopulateDisplayPeople(_firmService);
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
            seminar = _seminarService.GetCurrent();

            return seminar;
        }

        /// <summary>
        /// Assign people to sessions to attend.
        /// </summary>
        /// <param name="id">Seminar Id</param>
        /// <returns></returns>
        [UserOnly]
        public ActionResult AssignToSessions(int id)
        {
            var seminar = LoadSeminar(id);

            // redirect to the list if no seminar
            if (seminar == null)
            {
                ErrorMessages = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = AssignToSessionViewModel.Create(seminar);
            return View(viewModel);
        }

        /// <summary>
        /// Assigns a person into a session
        /// </summary>
        /// <param name="id">Session Id</param>
        /// <param name="seminarPersonId"></param>
        /// <returns></returns>
        [HttpPost]
        [UserOnly]
        public JsonResult Assign(int id, int seminarPersonId)
        {
            var session = _sessionRepository.GetNullableById(id);
            var seminarPerson = _seminarPersonRepository.GetNullableById(seminarPersonId);

            // make sure the sesion isn't already assigned
            if (!seminarPerson.Sessions.Any(a => a == session))
            {
                seminarPerson.Sessions.Add(session);
                _seminarPersonRepository.EnsurePersistent(seminarPerson);

                return Json(true);
            }

            return Json(false);
        }

        /// <summary>
        /// Removes a session from a person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="seminarPersonId"></param>
        /// <returns></returns>
        [HttpPost]
        [UserOnly]
        public JsonResult UnAssign(int id, int seminarPersonId)
        {
            //var session = _sessionRepository.GetNullableById(id);
            var seminarPerson = _seminarPersonRepository.GetNullableById(seminarPersonId);

            var session = seminarPerson.Sessions.Where(a => a.Id == id).FirstOrDefault();
            seminarPerson.Sessions.Remove(session);

            _seminarPersonRepository.EnsurePersistent(seminarPerson);
            return Json(true);
        }

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

            if (!_personService.HasAccess(seminarPerson.Person, seminarPerson.Seminar))
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
    }
}

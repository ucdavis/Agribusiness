using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
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
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;

        public SeminarController(IRepository<Seminar> seminarRepository, IRepository<Session> sessionRepository, IRepository<SeminarPerson> seminarPersonRepository)
        {
            _seminarRepository = seminarRepository;
            _sessionRepository = sessionRepository;
            _seminarPersonRepository = seminarPersonRepository;
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

            return View(seminar);
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
            seminar = _seminarRepository.Queryable.LastOrDefault();

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

        #endregion
    }
}

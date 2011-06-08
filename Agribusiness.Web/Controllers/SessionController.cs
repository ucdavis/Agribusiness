using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using AutoMapper;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Session class
    /// </summary>
    public class SessionController : ApplicationController
    {
	    private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<Seminar> _seminarRepository;
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;
        private readonly IRepositoryWithTypedId<User, Guid> _userRepository;
        private readonly IPersonService _personService;

        public SessionController(IRepository<Session> sessionRepository, IRepository<Seminar> seminarRepository, IRepository<SeminarPerson> seminarPersonRepository
                               , IRepositoryWithTypedId<User, Guid> userRepository, IPersonService personService)
        {
            _sessionRepository = sessionRepository;
            _seminarRepository = seminarRepository;
            _seminarPersonRepository = seminarPersonRepository;
            _userRepository = userRepository;
            _personService = personService;
        }

        [UserOnly]
        public ActionResult Create(int seminarId)
        {
            var viewModel = SessionViewModel.Create(Repository, seminarId);
            return View(viewModel);
        }

        [UserOnly]
        [HttpPost]
        public ActionResult Create(int seminarId, [Bind(Exclude="Seminar, SeminarPeople")]Session session)
        {
            // set the seminar
            session.Seminar = _seminarRepository.GetNullableById(seminarId);

            // clear any previous errors
            ModelState.Clear();

            // validate that a seminar was returned
            session.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _sessionRepository.EnsurePersistent(session);
                Message = string.Format(Messages.Saved, "Session");
                return this.RedirectToAction<SeminarController>(a => a.Edit(seminarId));
            }

            var viewModel = SessionViewModel.Create(Repository, seminarId, session);
            return View(viewModel);
        }

        [UserOnly]
        public ActionResult Edit(int id, int seminarId)
        {
            var session = _sessionRepository.GetNullableById(id);

            if (session == null)
            {
                ErrorMessages = string.Format(Messages.NotFound, "Session", id);
                return this.RedirectToAction<SeminarController>(a => a.Edit(seminarId));
            }

            var viewModel = SessionViewModel.Create(Repository, seminarId, session);
            return View(viewModel);
        }
        
        [UserOnly]
        [HttpPost]
        public ActionResult Edit(int id, int seminarId, [Bind(Exclude="Seminar, SeminarPeople")]Session session)
        {
            var origSession = _sessionRepository.GetNullableById(id);

            if (origSession == null)
            {
                ErrorMessages = string.Format(Messages.NotFound, "Session", id);
                return this.RedirectToAction<SeminarController>(a => a.Edit(seminarId));
            }

            Mapper.Map(session, origSession);

            // clear the modelstate and vaidate
            ModelState.Clear();
            origSession.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _sessionRepository.EnsurePersistent(origSession);
                Message = string.Format(Messages.Saved, "Session");
                return this.RedirectToAction<SeminarController>(a => a.Edit(seminarId));
            }

            var viewModel = SessionViewModel.Create(Repository, seminarId, session);
            return View(viewModel);
        }

        /// <summary>
        /// Add a seminar person
        /// </summary>
        /// <param name="id">Session Id</param>
        /// <returns></returns>
        [UserOnly]
        public ActionResult AddPerson(int id)
        {
            var session = _sessionRepository.GetNullableById(id);

            if (session == null)
            {
                Message = string.Format(Messages.NotFound, "session", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var viewModel = SessionPersonViewModel.Create(Repository, session);
            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Session Id</param>
        /// <param name="seminarPerson">Seminar Person</param>
        /// <returns></returns>
        [HttpPost]
        [UserOnly]
        public ActionResult AddPerson(int id, int seminarPersonId)
        {
            var session = _sessionRepository.GetNullableById(id);
            var seminarPerson = _seminarPersonRepository.GetNullableById(seminarPersonId);

            if (session == null)
            {
                Message = string.Format(Messages.NotFound, "session", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            if (seminarPerson == null)
            {
                Message = string.Format(Messages.NotFound, "seminar person", seminarPersonId);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            if (session.SessionPeople.Contains(seminarPerson))
            {
                ModelState.AddModelError("Person", "Person already exists in the session.");
            }

            if (ModelState.IsValid)
            {
                session.SessionPeople.Add(seminarPerson);
                _sessionRepository.EnsurePersistent(session);

                Message = string.Format(Messages.Saved, "Session person");
                return this.RedirectToAction<SessionController>(a => a.Edit(id, session.Seminar.Id));
            }

            var viewModel = SessionPersonViewModel.Create(Repository, session, seminarPerson);
            return View(viewModel);
        }

        /// <summary>
        /// Remove a seminar person
        /// </summary>
        /// <param name="id">Session Id</param>
        /// <param name="seminarPersonId">Seminar Person Id</param>
        /// <returns></returns>
        [HttpPost]
        [UserOnly]
        public ActionResult RemovePerson(int id, int seminarPersonId)
        {
            var session = _sessionRepository.GetNullableById(id);
            var seminarPerson = _seminarPersonRepository.GetNullableById(seminarPersonId);

            if (session == null)
            {
                Message = string.Format(Messages.NotFound, "session", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            if (seminarPerson == null)
            {
                Message = string.Format(Messages.NotFound, "seminar person", seminarPersonId);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            session.SessionPeople.Remove(seminarPerson);
            _sessionRepository.EnsurePersistent(session);

            Message = string.Format(Messages.Saved, "Session");
            return this.RedirectToAction<SessionController>(a => a.Edit(id, session.Seminar.Id));
        }


        #region Membership Functions
        /// <summary>
        /// Session details for attendees
        /// </summary>
        /// <param name="id">Session Id</param>
        /// <returns></returns>
        [MembershipUserOnly]
        public ActionResult Details(int id)
        {
            var session = _sessionRepository.GetNullableById(id);
            var user = _userRepository.Queryable.Where(a => a.LoweredUserName == CurrentUser.Identity.Name.ToLower()).SingleOrDefault();

            if (session == null)
            {
                Message = "Unable to locate requested session.  Please try again.";
                return this.RedirectToAction<AuthorizedController>(a => a.Index());
            }

            if (user == null || !_personService.HasAccess(user.Person, session.Seminar)) return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());

            TempData["SeminarPerson"] = user.Person.SeminarPeople.Where(a => a.Seminar == session.Seminar).FirstOrDefault();

            return View(session);
        }
        #endregion
    }
}

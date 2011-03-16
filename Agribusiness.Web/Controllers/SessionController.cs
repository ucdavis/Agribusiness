using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
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

        public SessionController(IRepository<Session> sessionRepository, IRepository<Seminar> seminarRepository)
        {
            _sessionRepository = sessionRepository;
            _seminarRepository = seminarRepository;
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
            foreach(var a in ModelState.Values) a.Errors.Clear();

            // validate that a seminar was returned
            session.TransferValidationMessagesTo(ModelState);

            //if (ModelState.IsValid)
            //{
            //    _sessionRepository.EnsurePersistent(session);
            //    Message = string.Format(Messages.Saved, "Session");
            //    return this.RedirectToAction<SeminarController>(a => a.Edit(seminarId));
            //}

            var viewModel = SessionViewModel.Create(Repository, seminarId, session);
            return View(viewModel);
        }
    }
}

using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
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
    /// Controller for the Attendee class
    /// </summary>
    [Authorize]
    public class AttendeeController : ApplicationController
    {
        private readonly IRepository<Seminar> _seminarRespository;
        private readonly IRepositoryWithTypedId<User, Guid> _userRepository;
        private readonly IPersonService _personService;

        public AttendeeController(IRepository<Seminar> seminarRespository, IRepositoryWithTypedId<User, Guid> userRepository, IPersonService personService)
        {
            _seminarRespository = seminarRespository;
            _userRepository = userRepository;
            _personService = personService;
        }

        /// <summary>
        /// Attendee List for a seminar
        /// </summary>
        /// <param name="id">seminar id</param>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            var seminar = _seminarRespository.GetNullableById(id);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction("Index", "Seminar");
            }

            var viewModel = AttendeeListViewModel.Create(seminar, _personService);
            return View(viewModel);
        }

        #region Membership Users
        /// <summary>
        /// List of attendees by seminar
        /// </summary>
        /// <param name="id">Seminar Id</param>
        /// <returns></returns>
        [MembershipUserOnly]
        public ActionResult BySeminar(int id)
        {
            var seminar = _seminarRespository.GetNullableById(id);

            // get the user's seminar person id
            var user = _userRepository.Queryable.Where(a => a.LoweredUserName == CurrentUser.Identity.Name.ToLower()).SingleOrDefault();
            if (user == null) return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());

            var person = user.Person;
            var latestReg = person.GetLatestRegistration();

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "seminar", id);
                return this.RedirectToAction<SeminarController>(a => a.MySeminar(latestReg.Id));
            }

            // validate seminar access
            if (!seminar.SeminarPeople.Contains(latestReg)) return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());

            ViewBag.seminarPersonId = latestReg.Id;

            return View(_personService.GetDisplayPeopleForSeminar(seminar.Id));
        }

        /// <summary>
        /// Profile page for attendees to view
        /// </summary>
        /// <param name="id">Person Id</param>
        /// <returns></returns>
        [MembershipUserOnly]
        public ActionResult Profile(int id)
        {
            return View();
        }
        #endregion
    }
}
